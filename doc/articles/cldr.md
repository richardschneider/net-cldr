# Unicode Common Locale Data Repository

The Unicode CLDR provides the documents (XML files) for software to support the world's languages. It 
uses the [Unicode Locale Data Markup Language (LDML)](https://www.unicode.org/reports/tr35/tr35-53/tr35.html) 
for the exchange of structured [locale](locale.md) data.

The [Cldr.Instance](xref:Sepia.Globalization.Cldr), a singleton, provides methods for 
- [downloading](cldr/downloading.md) the CLDR
- [extending](cldr/extending.md) the CLDR
- [getting](cldr/get-docs.md) the documents
- [searching](cldr/searching.md) the documents via XPATH expressions.