using RemoteCsv.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public class BoolParser : IFieldParser
    {
        public bool ParseField(object obj, FromCsvAttribute attribute, FieldInfo field, in List<List<string>> data, ref int lastRowIndex)
        {
            var result = ParseValue(attribute, in data, ref lastRowIndex, out var value);

            if (result)
                field.SetValue(obj, value);

            return result;
        }

        public bool ParseValue(FromCsvAttribute attribute, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null)
        {
            var rowIndex = this.GetActualRowIndex(attribute.RowIndex, ref lastRowIndex);
            var columnIndex = attribute.ColumnIndex;

            if (rowIndex < data.Count)
            {
                if (columnIndex < data[rowIndex].Count)
                {
                    if (bool.TryParse(data[rowIndex][columnIndex], out var result))
                    {
                        value = result;
                        return true;
                    }
                    else
                    {
                        Logger.LogError($"Can`t parse '{data[rowIndex][columnIndex]}' to bool!");
                    }
                }
                else
                {
                    Logger.LogWarning("ColumnIndex is out of range!");
                }
            }
            else
            {
                Logger.LogWarning("RowIndex is out of range!");
            }

            value = false;
            return false;
        }
    }
}
