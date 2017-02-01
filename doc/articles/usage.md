# Usage

    using Makaretu.Globalization;

The CLDR uses the singleton pattern.  To access the repository, use `Cldr.Instance`.

## Download CLDR

Ensure that the latest version of the  CLDR is installed.  This will download the latest published release from [unicode.org](http://www.unicode.org/Public/cldr/latest), if required.

    var version = Cldr.Instance.DownloadLatestAsync().Result;
    Console.WriteLine($"Using CLDR {version}");
