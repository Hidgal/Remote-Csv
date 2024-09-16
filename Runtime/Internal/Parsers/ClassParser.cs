using RemoteCsv.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public class ClassParser : IFieldParser
    {
        public bool ParseField(object obj, FieldInfo field, in List<List<string>> data, ref int lastRowIndex)
        {
            var attribute = field.GetCsvAttribute();
            var result = ParseValue(attribute.Column, attribute.Row, attribute.ItemsCount, in data, ref lastRowIndex, out var value, obj.GetType());

            if (result)
                field.SetValue(obj, value);

            return result;
        }

        public bool ParseValue(int columnIndex, int rowIndex, int rowEndIndex, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null)
        {
            if(type != null)
            {
                rowIndex = this.GetActualRowIndex(rowIndex, ref lastRowIndex);

                if (rowIndex < data.Count)
                {
                    if (columnIndex < data[rowIndex].Count)
                    {
                        IFieldParser parser;
                        bool result = false;
                        var savedRowIndex = lastRowIndex;
                        value = Activator.CreateInstance(type);
                        var fields = type.GetFieldsWithCsvAttribute();

                        foreach (var field in fields)
                        {
                            try
                            {
                                parser = ParserContainer.GetParser(field);
                                result |= parser.ParseField(value, field, in data, ref rowIndex);
                                rowIndex--;
                            }
                            catch (Exception e)
                            {
                                Logger.LogError(e.Message);
                            }
                        }

                        return result;
                    }
                }
            }

            value = default;
            return false;
        }
    }
}

