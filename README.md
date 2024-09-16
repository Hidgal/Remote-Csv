## Table Of Contents
<details>
<summary>Details</summary>
  
  - [Installation](#installation)
  - [Terminology](#terminology)
  - [How It Works](#how-it-works)
  - [How To Use](#how-to-use)
  - [Limitations](#limitations)
  - [Attribute Arguments](#attribute-arguments)
  - [Examples](#examples)

</details>

## Installation
 - Go to **Package Manager**
 - Press **+** and select **Add package from git URL...**

Paste following link in field:
```
https://github.com/Hidgal/Remote-Csv.git
```

## Terminology
**Row** - horizontal line of data in CSV document. In Sheets marked as number on side bar.
**Column** - vertical line of data in CSV document. In Sheets marked as Letter on top bar.

## How it works
The parser downloads the CSV file from the URL and places it in a special folder (**Assets/RemoteCsv** by default). The file will have the same name as the **ScriptableObject** associated with it.

It then splits the file into strings (one cell in sheet == one string) into a List<List<string>> using **[CSV Parser](https://github.com/yutokun/CSV-Parser) by yutokun**

Then for each ScriptableObject specified in the **Remote Scriptables List**, the parser gets a list of all fields with the **[FromCsv]** attribute.

After parsing whole field, the parser moves to the next row in the table. So the parser reads the fields row by row.

In case of **array parsing**, the parser moves to the next row only after it has finished parsing the entire array element.

If the row value has been overridden for a field, the parser will take the value from the specified row and then return to the last unread row.

## How To Use
1. Add **[FromCsv()]** attribute to necessary fields in your ScriptableObject class.
In arguments for attribute set information about Column, Row(optional) and Items Count(optional for array)
3. **Right click** in ScriptableObject asset inspector => **Add To Remotes List**
Or use **Tools -> Remote Csv -> Collect All Remotes** to find all available to parse Scriptable Objects in project.
4. Paste URL of CSV page in Google Sheets document to **Url field in Remote Scritpable List**
5. To parse file **Right click** in ScriptableObject asset inspector -> **Parse From Csv**. 
Or use **Tools -> Remote Csv -> Refresh All** to load and parse all Scriptable Objects in **Remote Scriptables List**.

## Limitations
In case of collections, only array is supported.

## Attribute Arguments
**FromCsv** attribute has several arguments to pass:

**column** - index of column. May be enum (Column.A) or integer (starts from 1, e.g. column A is 1 ant etc.)

**row** - (optional) row override (starts from 1). Allows you to override row index for current field.

**itemsCount** - (optional for array) override items count to parse in array. By default the parser goes through all rows ​​up to the end of the document.

## Examples
```
[System.Serializable]
public class ParseableTest
{
   [SerializeField]
   [FromCsv(Column.B)]
   private int _index;

   [SerializeField]
   [FromCsv(Column.C)]
   private string _description;
}

[SerializeField]
[FromCsv(Column.A)]
private int _testInt;

[SerializeField]
[FromCsv(row: 3, itemsCount: 5)]
private ParseableTest[] _parse;
```
