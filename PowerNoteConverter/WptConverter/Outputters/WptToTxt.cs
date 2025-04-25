namespace WptConverter.Outputters
{
    public class WptToTxt
    {
        //TODO: need a system for crawling through the file byte by byte.
        //We will have a list of control codes (that are not standard ANSI or UTF-8)
        //and if we don't encounter a control code we can convert the byte to UTF-8
        //but if it's a control code we will need to look a head to see what other
        //control codes are present in order to determine what character to return.
    }
}
