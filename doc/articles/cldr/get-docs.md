# Getting Documents

CLDR documents are XML files represented as a [XPathDocument](xref:System.Xml.XPath.XPathDocument) 
and may exist in multiple [repositories](xref:Sepia.Globalization.Cldr.Repositories).

[GetDocuments](xref:Sepia.Globalization.Cldr.GetDocuments*) returns a 
sequence of XPath documents that match the supplied name.  For performance 
reasons, a cache of XPath documents is maintained.

```csharp
var docs = Cldr.Instance.GetDocuments("common/supplemental/supplementalData.xml");
```
