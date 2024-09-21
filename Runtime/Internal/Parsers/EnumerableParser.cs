using RemoteCsv.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

            var startRowIndex = attribute.Row <= 0 ? lastRowIndex : attribute.Row;
            var itemsCount = attribute.ItemsCount <= 0 ? data.Count - startRowIndex : Mathf.Min(attribute.ItemsCount, data.Count);

            try
            {
                var array = Array.CreateInstance(elementType, itemsCount);
                for (int i = 0; i < array.Length; i++)
                {
                    isParsed |= parser.ParseValue(attribute.Column, i + startRowIndex, itemsCount, in data, ref lastRowIndex, out value, elementType);
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

            lastRowIndex = Mathf.Max(lastRowIndex, startRowIndex + itemsCount);
            return isParsed;
        }

        public bool ParseValue(int columnIndex, int rowIndex, int itemsCount, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null)
        {
            throw new Exception("Can`t parse enumerable value!");
        }
    }
}
