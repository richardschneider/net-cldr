# Makaretu.Globalization

[![build status](https://ci.appveyor.com/api/projects/status/github/richardschneider/net-cldr?branch=master&svg=true)](https://ci.appveyor.com/project/richardschneider/net-cldr) 
[![Coverage Status](https://coveralls.io/repos/richardschneider/net-cldr/badge.svg?branch=master&service=github)](https://coveralls.io/github/richardschneider/net-cldr?branch=master)
[![NuGet](https://img.shields.io/nuget/v/Makaretu.Globalization.svg)](https://www.nuget.org/packages/Makaretu.Globalization)
[![docs](https://cdn.rawgit.com/richardschneider/net-cldr/master/doc/images/docs-latest-green.svg)](https://richardschneider.github.io/net-cldr)

Provides locale content for internationalisation software using the Unicode Common Local Data Repository (CLDR).

## Features

- Uses the official [Unicode CLDR XML](http://www.unicode.org/Public/cldr/) without any "build steps"
- Can download the latest CLDR anytime
- Uses [XPath expressions](https://msdn.microsoft.com/en-us/library/ms256471(v=vs.110).aspx) to query the CLDR
- Parsing of a locale identifier
- Validation of common codes (language, currency, ...)
- Supports complex CLDR inheritance via
  - [locale truncation](http://unicode.org/reports/tr35/tr35.html#Locale_Inheritance)
  - [parent locale](http://unicode.org/reports/tr35/tr35.html#Parent_Locales)

## Getting started

Published releases of the package are available on [NuGet](https://www.nuget.org/packages/Makaretu.Globalization/).  To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console).

    PM> Install-Package Makaretu.Globalization

### Usage

    using Makaretu.Globalization;

The CLDR uses the singleton pattern.  To access the repository, use `Cldr.Instance`.

Ensure that the latest version of the  CLDR is installed.  This will download the latest published release from [unicode.org](http://www.unicode.org/Public/cldr/latest), if required.

    var version = Cldr.Instance.DownloadLatestAsync().Result;
    Console.WriteLine($"Using CLDR {version}");

Query the CLDR for the fractional digits of the Japanese Yen (JPY)

    var jpy = Cldr.Instance
        .GetDocuments("common/supplemental/supplementalData.xml")
        .FirstElement("supplementalData/currencyData/fractions/info[@iso4217='JPY']");
    Assert.AreEqual("0", jpy.Attribute("digits").Value);


