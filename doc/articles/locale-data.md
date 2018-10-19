# Locale Data

Finding locale specific data requires searching multiple documents that are associated with 
the locale; see [Locale Inheritance](http://unicode.org/reports/tr35/#Locale_Inheritance) 
for the gory details.

## Search Chain 

[SearchChain](xref:Sepia.Globalization.Locale.SearchChain*) provides the names that 
are ordered from the most specific subtag to least specific. [ResourceBundle](xref:Sepia.Globalization.Locale.ResourceBundle*) 
provides the fully qualifed document name in a locale folder, such as `common/main/`, that need to be searched.


| ID      | Search chain | Comments |
| -----   | ------------ | -------- |
| en      | en_Latn_US en_US en_Latn en root | Defaults the [region](xref:Sepia.Globalization.LocaleIdentifier.Region) to `US` and [script](xref:Sepia.Globalization.LocaleIdentifier.Script) to `Latn` |
| en-NZ   | en_Latn_NZ en_NZ en_001 en root | Defaults the [script](xref:Sepia.Globalization.LocaleIdentifier.Script) to `Latn` and also uses the World's (en_001) data |
| zh-MO   | zh_Hant_MO zh_Hant_HK zh_HK zh_Hant root | Uses Macau and then Hong Kong data |

## Findng Data

The following methods are used to find some locale specific data with 
a XPath expression.

| Method | Description |
| ------ | ----------- |
| [Find](xref:Sepia.Globalization.Locale.Find*) |  Find the first element that matches |
| [FindOrDefault](xref:Sepia.Globalization.Locale.FindOrDefault*) | Find the first element that matches or return the default value |

## Example

Find the format string to display a gram unit in the plural form.

```csharp
var locale = Locale.Create("fr-CA");
var plural = locale
  .Find(@"ldml/units/unitLength[@type='long']/unit[@type='mass-gram']/unitPattern[@count='other']")
  .Value;
Assert.AreEqual("{0} grammes", plural);
```
