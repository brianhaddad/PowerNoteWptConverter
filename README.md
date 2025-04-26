# PowerNoteWptConverter
A converter to read WPT files and output to TXT (with RTF being a future possibility). Tested with the Brother PN-8500MDS.

If you build the WptToTxtUtility targeting your system (I have included an executable built for windows x64 systems in the root directory of this repository) you will get a command line executable that is designed to have files dragged and dropped onto it for conversion. You can drag one or multiple files and the program will output .txt files in the same directory as the files you dragged in.

Because I do not use most of the fancy formatting options (I don't even use tabs usually), my use case was simple. I am extracting only the main text characters. I still had to deal with a header (whose information is a mystery to me) and some strange characters the system inserts into the files such as line breaks within paragraphs that I interpret as spaces. Aside from a few anomalies, the format is largely compatible with UTF-8/ASCII.

A few improvements I may make:

* First, I may make it so that running the executable within a directory will result in any files in that directory being converted in place. I may also create a settings file that would allow a target output directory to be specified by the user prior to running.

* Next I would like to decode more of the special control characters. I made an attempt at handling tabs, but I got two different sets of control characters associated with hitting the tab key in my test files. I handled those two, but I have no idea if future files containing tabs will be interpreted correctly. In fact, I have an old test file with some strange tab related control characters that I am not currently handling properly, which is why I added a filter just before the final output to ensure stray unhandled control codes don't end up in the final output.

* Eventually I think it should be possible to create an RTF exporter that handles in-file formatting such as bold, italic, etc. but that is beyond my personal use case. If there is enough interest in such a capability I could be convinced to work on the feature with a little input from a supportive community.

Finally, if anyone has any technical information about the header data in the WPT file format I would love to read that data into my program in a way that could actually help decode the file. My simple test files had one byte that would occasionally be different between them, and I do not know what that byte means. Similarly, if anyone has a list of the control characters Brother was using around that time and what they meant, such information would be immensely useful to me!

Note that the license for this project allows for all kinds of personal forking and remixing but if this code ends up in any commercial projects that would be a violation of the license. I want this to be a free public utility for users of the Brother PowerNote.

Many thanks to [ToughDev](https://www.toughdev.com/) for linking to this repository from a [blog post about the Brother Super PowerNote PN-8500MDSe](https://www.toughdev.com/content/2016/06/brother-super-powernote-pn-8500mdse-vintage-word-processor/). That post has long been a gathering place for individuals interested in the Brother Super PowerNote where the comments have become a kind of forum for sharing and exchanging information. Thank you!
