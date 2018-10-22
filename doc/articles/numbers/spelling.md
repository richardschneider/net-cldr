# Spelling a number

The [SpellingFormatter](xref:Sepia.Globalization.Numbers.SpellingFormatter) is used to 
[spellout](xref:Sepia.Globalization.Numbers.SpellingFormatter.Format*) a number (long, decimal or double) in a 
[locale](../locale.md) specific manner. The [Create method](xref:Sepia.Globalization.Numbers.SpellingFormatter.Create*) 
is used to create a formatter for a specific locale and a set of 
[options](xref:Sepia.Globalization.Numbers.SpellingOptions).

```csharp
var locale = Locale.Create("fr");
var formatter = NumberFormatter.Create(locale);

Assert.AreEqual("moins un", formatter.Format(-1));
Assert.AreEqual("zéro", formatter.Format(0));
Assert.AreEqual("vingt-et-un", formatter.Format(21));
Assert.AreEqual("quatre-vingt-dix-neuf", formatter.Format(99));
Assert.AreEqual("un virgule deux", formatter.Format(1.2));
```
## Options

[SpellingOptions](xref:Sepia.Globalization.Numbers.SpellingOptions) are used to 
determine the [style](xref:Sepia.Globalization.Numbers.SpellingStyle) for 
spelling out a number.

| Style    | en    | fr |
| -------- | ----  | ----- |
| Cardinal | one   | un |
| Ordinal  | first | unième |

