# Formatting a number

An [INumberFormatter](xref:Sepia.Globalization.Numbers.INumberFormatter) is used to 
[Format](xref:Sepia.Globalization.Numbers.INumberFormatter.Format*) a number (long, decimal or double) in a 
[locale](../locale.md) specific manner. [NumberFormatter.Create](xref:Sepia.Globalization.Numbers.NumberFormatter.Create*) 
is used to create a formatter for a specific locale and a set of 
[options](xref:Sepia.Globalization.Numbers.NumberOptions).

```csharp
var locale = Locale.Create("fr");
var formatter = NumberFormatter.Create(locale);

Assert.AreEqual("123", formatter.Format(123));
Assert.AreEqual("1234", formatter.Format(1234));
Assert.AreEqual("1234,568", formatter.Format(1234.56789));
```

## Options

[NumberOptions](xref:Sepia.Globalization.Numbers.NumberOptions) is used to 
determine the [style](xref:Sepia.Globalization.Numbers.NumberStyle) for formatting numeric quantities
and if digit grouping seperators should be used.

| Style | Format (en-GB) |
| ----- | ----------- |
| Decimal | -12,345 |
| CurrencyAccounting | (£12,345.00) |
| CurrencyStandard | -£12,345.00 |
| Percent | -1,234,500% |
| Scientific | -1.2345E4 |

