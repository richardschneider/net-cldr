# Locale ID

A [locale ID](xref:Sepia.Globalization.LocaleIdentifier) is used to name a set preferences for a
[locale](locale.md).  The identifier is based on 
[BCP47](https://www.rfc-editor.org/rfc/bcp/bcp47.txt) and 
[Unicode TR 35](http://www.unicode.org/reports/tr35/#Unicode_locale_identifier) 
for distinguishing among languages, country/region, currencies, 
time zones, transforms, and so on.

Typically, the ID is a [language code](https://en.wikipedia.org/wiki/ISO_639) or 
a language and [region code](https://en.wikipedia.org/wiki/ISO_3166-1); 
such as `en` and `en-NZ`, respectively. However, ID's can also contain 
a [script](xref:Sepia.Globalization.LocaleIdentifier.Script), 
[extensions](xref:Sepia.Globalization.LocaleIdentifier.Extensions) and
[variants](xref:Sepia.Globalization.LocaleIdentifier.Variants).

## Parsing

The [Parse](xref:Sepia.Globalization.LocaleIdentifier.Parse*) and 
[TryParse](xref:Sepia.Globalization.LocaleIdentifier.TryParse*) methods are used to convert the 
string representation of the ID into the various subtags. 
[MostLikelySubtags](xref:Sepia.Globalization.LocaleIdentifier.MostLikelySubtags*) returns 
a new ID with the [language](xref:Sepia.Globalization.LocaleIdentifier.Language), 
[script](xref:Sepia.Globalization.LocaleIdentifier.Script) and 
[region](xref:Sepia.Globalization.LocaleIdentifier.Region) filled in with a likely value.

| ID      | Most Likely | Description |
| ------- | ----------- | ----------- |
| `en`    | en-Latn-US  | English |
| `en-NZ` | en-Latn-NZ  | English in New Zealand |
| `zh`    | zh-Hans-CN  | Simplified Chinese |
| `zh-TW` | zh-Hant-TW  | Traditional Chinese in Taiwan |

