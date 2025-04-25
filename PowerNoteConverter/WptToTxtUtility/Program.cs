using WptConverter.Outputters;

Console.WriteLine("Running file conversions...");

var inputFilePaths = Environment.GetCommandLineArgs()
    .Where(x => Path.GetExtension(x).Equals(".WPT", StringComparison.CurrentCultureIgnoreCase))
    .ToArray();

if (inputFilePaths.Length > 0)
{
    Console.WriteLine("Input file paths: ");
    foreach (var path in inputFilePaths)
    {
        Console.WriteLine($"\t{path}");
    }

    Console.WriteLine("Outputs: ");
    foreach (var path in inputFilePaths)
    {
        var folder = Path.GetDirectoryName(path);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);

        WptToTxt wptToTxt = new();
        var output = Path.Combine(folder, $"{fileNameWithoutExtension}.txt");
        wptToTxt.Process(path, output);
        Console.WriteLine($"\t{output}");
        Console.WriteLine($"Header Bytes: {string.Join(" ", wptToTxt.HeaderBytes.Select(x => x.ToString("X2")))}");
        if (wptToTxt.EofError)
        {
            Console.WriteLine("EOF Error. Somehow reached the EOF character before the actual end of the bytestream.");
        }
    }
}

#if DEBUG
//Note: this is just local test code. Feel free to change the path values or play around with the output.
Console.WriteLine("Begin test.");
var inputPath = @"C:\Users\brian\Dropbox\projects\bit_banging\WPT\LONGTEST.WPT";
var outputPath = @"C:\Users\brian\Dropbox\projects\bit_banging\WPT\LongTestOutput.txt";
WptToTxt converter = new();
converter.Process(inputPath, outputPath);
Console.WriteLine("Done.");
Console.WriteLine("Header bytes:");
Console.WriteLine(string.Join(" ", converter.HeaderBytes.Select(x => x.ToString("X2"))));
if (converter.EofError)
{
    Console.WriteLine("EOF Error. Somehow reached the EOF character before the actual end of the bytestream.");
}
#endif

Console.WriteLine("Done. Press enter to close the window.");
Console.ReadLine();