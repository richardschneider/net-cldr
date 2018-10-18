# Searching Documents

[CldrExtensions](xref:Sepia.Globalization.CldrExtensions) provides the methods to 
search mulitple XPath documents with a XPath expression.

| Method | Description |
| ------ | ----------- |
| [Elements](xref:Sepia.Globalization.CldrExtensions.Elements*) | Find all the elements that match |
| [FirstElement](xref:Sepia.Globalization.CldrExtensions.FirstElement*) |  Find the first element that matches |
| [FirstElementOrDefault](xref:Sepia.Globalization.CldrExtensions.FirstElementOrDefault*) | Find the first element that matches or return the default value |

## Example

Query the CLDR for the number of fractional digits of the Japanese Yen

```csharp
var jpy = Cldr.Instance
  .GetDocuments("common/supplemental/supplementalData.xml")
  .FirstElement("supplementalData/currencyData/fractions/info[@iso4217='JPY']");
Assert.AreEqual("0", jpy.Attribute("digits").Value);
```
