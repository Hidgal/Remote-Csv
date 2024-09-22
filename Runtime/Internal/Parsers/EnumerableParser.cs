using System;   
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RemoteCsv.Internal.Parsers
{
    public class EnumerableParser : IFieldParser
    {
        public bool ParseField(object obj, FromCsvAttribute attribute, FieldInfo field, in List<List<string>> data, ref int lastRowIndex)
        {
            var result = ParseValue(attribute, in data, ref lastRowIndex, out var value, field.FieldType);
            
            if (result)
                field.SetValue(obj, value);

            return result;
        }

        public bool ParseValue(FromCsvAttribute attribute, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null)
        {
            Type elementType = type.GetElementType();
            value = default;

            if (elementType != null)
            {
                var isParsed = false;
                var elementParser = ParserContainer.GetParser(elementType, attribute);

                var arrayAttribute = attribute.Clone();
                var startRowIndex = arrayAttribute.RowIndex <= 0 ? lastRowIndex : arrayAttribute.RowIndex;
                var itemsCount = arrayAttribute.ItemsCount <= 0 ? data.Count - startRowIndex : Mathf.Min(arrayAttribute.ItemsCount, data.Count);
                arrayAttribute.SetItemsCount(itemsCount);

                try
                {
                    var array = Array.CreateInstance(elementType, itemsCount);
                    for (int i = 0; i < array.Length; i++)
                    {
                        arrayAttribute.SetRowIndex(i + startRowIndex);
                        isParsed |= elementParser.ParseValue(arrayAttribute, in data, ref lastRowIndex, out value, elementType);
                        array.SetValue(value, i);
                    }

                    value = array;
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                }

                lastRowIndex = Mathf.Max(lastRowIndex, startRowIndex + itemsCount);
                return isParsed;
            }

            Logger.LogError("Can`t get element type for array parser");
            return false;
        }
    }
}
