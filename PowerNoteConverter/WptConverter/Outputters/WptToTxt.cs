namespace WptConverter.Outputters
{
    public class WptToTxt
    {
        private readonly List<byte> _headerBytes = [];
        public byte[] HeaderBytes => _headerBytes.ToArray();

        public bool EofError { get; private set; } = false;

        //Some of these are pretty clear from my test files, others (especially anything related to tabs)
        //are difficult to be sure of. This will require some additional testing and refining I think.
        private const byte CARRIAGE_RETURN = 0xBD;
        private const byte LINE_FEED = 0x02;
        private const byte LINE_BREAK = 0xDB;
        private const byte UNKNOWN_TAB_INSTRUCTION = 0xBC;
        private const byte POSSIBLE_TAB_EXECUTE = 0x01;
        private const byte NEW_LINE_TAB_START = 0xDD;
        private const byte CENTS_BYTE = 0x20;
        private const byte SPACE = 0x00;
        private const byte EOF = 0xDE;
        private const char CENTS_CHAR = (char)0xA2; //Cents symbol

        //TODO: someday I can refactor this into different parts.
        //For now it is all one class for simplicity.

        public void Process(string wptFilePath, string txtOutputPath)
        {
            List<byte> bytes = [];

            try
            {
                using (FileStream fs = new FileStream(wptFilePath, FileMode.Open, FileAccess.Read))
                {
                    int byteRead;
                    while ((byteRead = fs.ReadByte()) != -1)
                    {
                        bytes.Add((byte)byteRead);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            var resultingCharacters = ProcessBytes(bytes.ToArray());

            File.WriteAllLines(txtOutputPath, resultingCharacters);
        }

        private string[] ProcessBytes(byte[] bytes)
        {
            List<string> lines = [];

            int i = 0;

            //The header is terminated by the first instance of CARRIAGE_RETURN
            while (bytes[i] != CARRIAGE_RETURN)
            {
                _headerBytes.Add(bytes[i]);
                i++;
            }

            //Advance beyond the initial carriage return.
            i++;

            var line = "";

            while (i < bytes.Length)
            {
                switch(bytes[i])
                {
                    //Mid paragraph line breaks are replaced with a space.
                    case LINE_BREAK:
                        i++;
                        if (bytes[i] == CARRIAGE_RETURN)
                        {
                            line += " ";
                            i++;
                        }
                        break;

                    //Actual line feeds are replaced with a new line.
                    //These can consist of multiple line feeds followed by a single carriage return.
                    //I found one instance where the carriage return was preceeded by what I assume
                    //is a tab caracter. Other tests with tab did not yield the same result.
                    case LINE_FEED:
                        while (bytes[i] == LINE_FEED)
                        {
                            lines.Add(line);
                            line = "";
                            i++;
                            if (bytes[i] == NEW_LINE_TAB_START)
                            {
                                line += "\t";
                                i++;
                            }
                        }
                        if (bytes[i] == CARRIAGE_RETURN)
                        {
                            i++;
                        }
                        break;

                    //Not sure how tab works exactly. Can modify after more testing.
                    case UNKNOWN_TAB_INSTRUCTION:
                        i++;
                        if (bytes[i] == POSSIBLE_TAB_EXECUTE)
                        {
                            line += "\t";
                            i++;
                        }
                        break;

                    case EOF:
                        lines.Add(line);
                        line = "";
                        i++;
                        if (i < bytes.Length)
                        {
                            //This should not happen since this character should mark the end of the file.
                            //We can assume there has been an error or we can keep going. Probably best to
                            //store the remaining data in an overflow of some kind?
                            //For now we will ignore the discrepancy and keep processing the data.
                            EofError = true;
                        }
                        break;

                    //The rest are just characters that for one reason or another are encoded
                    //differently from expected.

                    case CENTS_BYTE:
                        line += CENTS_CHAR;
                        i++;
                        break;

                    case SPACE:
                        line += " ";
                        i++;
                        break;

                    //Finally, most of the remaining characters should just be plain old ASCII.
                    default:
                        line += (char)bytes[i];
                        i++;
                        break;
                }
            }

            //Though the line should be empty, if it's not we will add the final line to the lines list.
            if (line != "")
            {
                lines.Add(line);
            }

            return lines.ToArray();
        }
    }
}
