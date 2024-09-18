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
> Note: It`s recomended to install **[UniTask by Cysharp](https://github.com/Cysharp/UniTask "UniTask")** via **Unity Package Manager** for better perfomance and code safety.

## Terminology
**Row** - horizontal line of data in CSV document. In Sheets marked as number on side bar.

**Column** - vertical line of data in CSV document. In Sheets marked as letter on top bar.

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
4. Paste straight URL to the CSV document to **Url field in Remote Scritpable List** (make sure the document is available to read and download using this link)
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
1. Simple int value and array int[] to parse
```csharp
[SerializeField]
[FromCsv(Column.B)]
private int[] _parsedValues;
[SerializeField]
[FromCsv(Column.D, row: 2)]
private int _anotherValue;
```
![Parse Result](https://i.imgur.com/VUI6l4p.png "Parse Result")

2. Array of serialiazable classes. 
> Note: if **itemsCount <= 0**, then the parser processes all the information from the csv file

```csharp
[System.Serializable]
public class TestParseClass
{
    [SerializeField]
    [FromCsv(Column.B)]
    private int _intValue;
            
    [SerializeField]
    [FromCsv(Column.C)]
    private string _stringValue;
}

[SerializeField]
[FromCsv(row: 2, itemsCount: 0)]
private TestParseClass[] _someData;
```
![Parse Result](https://i.imgur.com/SFb5nhi.png "Parse Result")

3. An example of complex parsing of different data

```csharp
[System.Serializable]
public class TestParseClass
{
    [SerializeField]
    [FromCsv(Column.B)]
    private int _intValue;
            
    [SerializeField]
    [FromCsv(Column.C)]
    private string _stringValue;
}

[SerializeField]
[FromCsv(row: 2, itemsCount: 2)]
private TestParseClass[] _someData;
[SerializeField]
[FromCsv(Column.B, row: 5)]
private int _anotherIntValue;
[SerializeField]
[FromCsv(Column.E, row: 2)]
private float _floatValue;
```
![Parse Result](https://i.imgur.com/HxGt5Oo.png "Parse Result")
