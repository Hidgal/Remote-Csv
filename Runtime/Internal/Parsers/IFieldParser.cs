using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public interface IFieldParser
    {
        bool ParseField(object obj, FieldInfo field, in List<List<string>> data, ref int lastRowIndex);
        bool ParseValue(int columnIndex, int rowIndex, int itemsCount, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null);
    }
}

