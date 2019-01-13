# Categories

Instead of using a specific numbering system name, a category name is
used to request a locale specific number system for the category.

| Category | Description |
| -------- | ----------- |
| native | Requests the numbering system used for the native digits, usually defined as a part of the script used to write the language. |
| financial | Requests the numbering system used for financial quantities. This is often used for ideographic languages such as Chinese, where it would be easy to alter an amount represented in the default numbering system simply by adding additional strokes. If the financial numbering system is not specified, applications should use the default numbering system as a fallback. |
| traditional | Requests the traditional numerals for a locale. If the traditional numbering system is not defined, applications should use the native numbering system as a fallback. |
