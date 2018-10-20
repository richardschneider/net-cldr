# Formatting currency

A currency is typically represented by a [decimal number](xref:System.Decimal) and 
[currency code](https://en.wikipedia.org/wiki/ISO_4217) tuple. 
The [Format](xref:Sepia.Globalization.Numbers.INumberFormatter) method allows a second parameter that represents the currency code. 
It's Style should be either CurrencyStandard or CurrencyAccounting.

```csharp
var locale = Locale.Create("en");
foreach (var style in new[] { NumberStyle.CurrencyAccounting, NumberStyle.CurrencyStandard })
{
    var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = style });
    Console.WriteLine($"{style} {formatter.Format(-1234.56, "EUR")}");
    Console.WriteLine($"{style} {formatter.Format(-1234.56, "JPY")}");
    Console.WriteLine($"{style} {formatter.Format(-1234.56, "CNY")}");
    Console.WriteLine($"{style} {formatter.Format(-1234.56, "USD")}");
}
```

| Currency | Style              | Format (en) |
| -------- | ------------------ | ----------- |
| EUR      | CurrencyAccounting | (€1234.56) |
|          | CurrencyStandard   | -€1234.56 |
| JPY      | CurrencyAccounting | (¥1235) |
|          | CurrencyStandard   | -¥1235 |
| CNY      | CurrencyAccounting | (CN¥1234.56) |
|          | CurrencyStandard   | -CN¥1234.56 |
| USD      | CurrencyAccounting | ($1234.56) |
|          | CurrencyStandard   | -$1234.56 |
