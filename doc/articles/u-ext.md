# Unicode BCP 47 U Extension

The [locale extension](xref:Sepia.Globalization.LocaleExtension) is an 
[Unicode extension](xref:Sepia.Globalization.LocaleIdentifier.UnicodeExtension) for 
identifying Unicode [locale](locale.md)-based variations using [language tags](locale-id.md). 
The extension starts with `-u` followed by a sequence of name/value pairs.

For example, the [ID](locale-id.md) `th-u-nu-thai-ca-buddhist` specifies that the Buddhist calendar 
and the Thai numbering system is used.

The name/value pairs are defined in [Unicode TR 35](https://unicode.org/reports/tr35/tr35.html#Key_And_Type_Definitions_). 
Some common extension names are:

| Name | Description | Values |
| ---- | ----------- | ------ |
| `ca` | Calendar algorithm | [calendar.xml](http://www.unicode.org/repos/cldr/tags/latest/common/bcp47/calendar.xml) |
| `cf` | Currency format style | [currency.xml](http://www.unicode.org/repos/cldr/tags/latest/common/bcp47/currency.xml) |
| `ms` | Measurement system | [measure.xml](http://www.unicode.org/repos/cldr/tags/latest/common/bcp47/measure.xml) |
| `nu` | Numbering system | [number.xml](http://www.unicode.org/repos/cldr/tags/latest/common/bcp47/number.xml) |
