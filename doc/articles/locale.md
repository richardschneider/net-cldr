# Locale

A [locale](xref:Sepia.Globalization.Locale) is a set of preferences that tend to be shared across 
significant swaths of the world.  The specific preferences are based on the [locale ID](locale-id.md).

## Usage

- Formatting numbers
- Formatting currency values
- Formatting traditional numbering systems
- Translation of currency symbols and names
- Spelling out numbers

## Example

[Create](xref:Sepia.Globalization.Locale.Create*) a Chinese locale using financial numbers.  

```csharp
var locale = Locale.Create("zh-u-nu-finance");
```
