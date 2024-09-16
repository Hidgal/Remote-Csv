using RemoteCsv.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public class IntParser : IFieldParser
    {
        public bool ParseField(object obj, FieldInfo field, in List<List<string>> data, ref int lastRowIndex)
        {
            var attribute = field.GetCsvAttribute();
            var result = ParseValue(attribute.Column, attribute.Row, attribute.ItemsCount, in data, ref lastRowIndex, out var value);

            if(result)
                field.SetValue(obj, value);

            return result;
        }

        public bool ParseValue(int columnIndex, int rowIndex, int itemsCount, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null)
        {
            rowIndex = this.GetActualRowIndex(rowIndex, ref lastRowIndex);

            if (rowIndex < data.Count)
            {
                if (columnIndex < data[rowIndex].Count)
                {
                    if (int.TryParse(data[rowIndex][columnIndex], out var result))
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
