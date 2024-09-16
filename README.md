## Installation
 - Go to **Package Manager**
 - Press **+** and select **Add package from git URL...**

Paste following link in field:
```
https://github.com/Hidgal/Remote-Csv.git
```

## How To Use
1. Add **[FromCsv()]** attribute to fields in your scriptable class.
In arguments for attribute set information about Column, Row and Items Count (for array)
2. **Right click** in ScriptableObject asset inspector => **Add To Remotes List**
3. Paste URL to CSV page in Google Sheets document to **Url field in Remote Scritpable List**

## Example Code
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
