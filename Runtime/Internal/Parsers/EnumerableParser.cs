using RemoteCsv.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv.Internal.Parsers
{
    public class EnumerableParser : IFieldParser
    {
        public bool ParseField(object obj, FieldInfo field, in List<List<string>> data, ref int lastRowIndex)
        {
            Type elementType = field.FieldType.GetElementType();

            object value;
            var isParsed = false;
            var parser = ParserContainer.GetParser(elementType);
            var attribute = field.GetCsvAttribute();

            var startRowIndex = attribute.RowIndex <= 0 ? lastRowIndex : attribute.RowIndex - 1;
            var itemsCount = attribute.ItemsCount <= 0 ? data.Count : Mathf.Min(attribute.ItemsCount, data.Count);

            try
            {
                var array = Array.CreateInstance(elementType, itemsCount);
                for (int i = 0; i < array.Length; i++)
                {
                    isParsed |= parser.ParseValue(attribute.ColumnIndex, i + startRowIndex, i + lastRowIndex, in data, ref lastRowIndex, out value);
                    array.SetValue(value, i);
                }

                if (isParsed)
                {
                    field.SetValue(obj, array);
                }
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }

            return isParsed;
        }

        public bool ParseValue(int columnIndex, int rowStartIndex, int rowEndIndex, in List<List<string>> data, ref int lastRowIndex, out object value)
        {
            throw new Exception("Can`t parse enumerable value!");
        }
    }
}
