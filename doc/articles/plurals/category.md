# Category

The [plural categories](xref:Sepia.Globalization.Plurals.Plural.Category*) s
do not necessarily match the traditional grammatical 
categories. Instead, the categoy is determined by changes required in a phrase or 
sentence if a numeric placeholder changes value.

## Usage

```csharp
var locale = Locale.Create("ksh");
var plural = Plural.Create(locale);

Assert.AreEqual("zero", plural.Category(0));
Assert.AreEqual("one", plural.Category(1));
Assert.AreEqual("other", plural.Category(2));
```

## Examples

| Number | Category (en) | Category (cy) |
| ------ | ------------- | ------------- |
| 0 | other | zero |
| 1 | one | one |
| 2 | other | two |
| 3 | other | few |
| 4 | other | other |
| 5 | other | other |
| 6 | other | many |
| 7 | other | other |
| 8 | other | other |
| 9 | other | other |
