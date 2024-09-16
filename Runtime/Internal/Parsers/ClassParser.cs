using RemoteCsv.Internal.Extensions;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RemoteCsv.Internal.Parsers
{
    public class ClassParser : IFieldParser
    {
        public bool ParseField(object obj, FieldInfo field, in List<List<string>> data, ref int lastRowIndex)
        {
            var attribute = field.GetCsvAttribute();
            var result = ParseValue(attribute.ColumnIndex, attribute.RowIndex, attribute.ItemsCount, in data, ref lastRowIndex, out var value);

            if (result)
                field.SetValue(obj, value);

            return result;
        }

        public bool ParseValue(int columnIndex, int rowStartIndex, int rowEndIndex, in List<List<string>> data, ref int lastRowIndex, out object value)
        {
            if (rowStartIndex < 0)
                rowStartIndex = lastRowIndex;
            else
                lastRowIndex = Mathf.Max(lastRowIndex, rowStartIndex);

            if (rowStartIndex < data.Count)
            {
                if (columnIndex < data[rowStartIndex].Count)
                {
                    if (int.TryParse(data[rowStartIndex][columnIndex], out var result))
                    {
                        value = result;
                        return true;
                    }
                }
            }

            value = 0;
            return false;
        }
    }
}

