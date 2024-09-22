using RemoteCsv.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public class ClassParser : IFieldParser
    {
        public bool ParseField(object obj, FromCsvAttribute attribute, FieldInfo field, in List<List<string>> data, ref int lastRowIndex)
        {
            var result = ParseValue(attribute, in data, ref lastRowIndex, out var value, obj.GetType());

            if (result)
                field.SetValue(obj, value);

            return result;
        }

        public bool ParseValue(FromCsvAttribute attribute, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null)
        {
            if(type != null)
            {
                var rowIndex = this.GetActualRowIndex(attribute.RowIndex, ref lastRowIndex);
                var columnIndex = attribute.ColumnIndex;

                if (rowIndex < data.Count)
                {
                    if (columnIndex < data[rowIndex].Count)
                    {
                        IFieldParser parser;
                        bool result = false;
                        value = Activator.CreateInstance(type);
                        var fields = type.GetFieldsWithCsvAttribute();

                        foreach (var field in fields)
                        {
                            try
                            {
                                attribute = field.GetCsvAttribute();
                                parser = ParserContainer.GetParser(field, attribute);
                                result |= parser.ParseField(value, attribute, field, in data, ref rowIndex);
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

