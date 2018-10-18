# Downloading the CLDR

For performance reasons, a local copy of the CLDR is at `%localappdata%/UnicodeCLDR`. 

## Getting the latest

The 
[DownloadLatestAsync](xref:Sepia.Globalization.Cldr.DownloadLatestAsync) method is used to fetch 
the compressed CLDR from 
[http://unicode.org/Public/cldr/](http://unicode.org/Public/cldr/) 
if the [current version](xref:Sepia.Globalization.Cldr.CurrentVersion) 
is less than the [latest version](xref:Sepia.Globalization.Cldr.LatestVersionAsync).

```csharp
var version = await Cldr.Instance.DownloadLatestAsync();
Console.WriteLine($"Using CLDR {version}");
```
## Folder layout

```
C:\USERS\OWNER\APPDATA\LOCAL\UNICODECLDR
├───core
│   └───common
│       ├───annotations
│       ├───annotationsDerived
│       ├───bcp47
│       ├───casing
│       ├───collation
│       ├───dtd
│       ├───main
│       ├───properties
│       ├───rbnf
│       ├───segments
│       ├───subdivisions
│       ├───supplemental
│       ├───transforms
│       ├───uca
│       └───validity
└───keyboards
    └───keyboards
        ├───android
        ├───chromeos
        ├───dtd
        ├───osx
        ├───und
        └───windows
```