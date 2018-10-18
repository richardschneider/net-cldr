# Extending the CLDR

[Repositories](xref:Sepia.Globalization.Cldr.Repositories) is a list of fully qualified directories 
that contais some CLDR files.  It 
is initially set to the [repo provided by Unicode](downloading.md).

If you want to add/change CLDR data, add a new repo path at the head of the list.

```csharp
var fqn = Path.Combine("...", "my-cldr");
Cldr.Instance.Repositories.Insert(0, fqn);
```
