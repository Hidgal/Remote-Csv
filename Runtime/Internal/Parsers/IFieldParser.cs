using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public interface IFieldParser
    {
        bool ParseField(object obj, FieldInfo field, in List<List<string>> data, ref int lastRowIndex);
        bool ParseValue(int columnIndex, int rowStartIndex, int rowEndIndex, in List<List<string>> data, ref int lastRowIndex, out object value);
    }
}

