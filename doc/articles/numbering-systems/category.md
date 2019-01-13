# Categories

Instead of using a specific numbering system name, a category name can be
used to request a locale specific numbering system for the category.

| Category | Description |
| -------- | ----------- |
| native | Requests the numbering system used for the native digits, usually defined as a part of the script used to write the language. |
| finance | Requests the numbering system used for financial quantities. This is often used for ideographic languages such as Chinese, where it would be easy to alter an amount represented in the default numbering system simply by adding additional strokes. |
| traditio | Requests the traditional numerals for a locale. If the traditional numbering system is not defined, applications should use the native numbering system as a fallback. |

If the locale does not define a numbering system for the category, then the default numbering system is used as a fallback.

## Example

The following table shows the number `123` represented in various locales.

| Locale | Formatted |
| ------ | --------- |
| en | 123 |
| en-u-nu-native | 123 |
| en-u-nu-traditio | 123 |
| en-u-nu-finance | 123 |
| zh-TW | 123 |
| zh-TW-u-nu-native | 一二三 |
| zh-TW-u-nu-traditio | 一百二十三 |
| zh-TW-u-nu-finance | 壹佰貳拾參 |
