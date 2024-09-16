using RemoteCsv.Internal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using yutokun;

namespace RemoteCsv.Internal.Parsers
{
    public class ObjectParser
    {
        /// <returns><see langword="true"/> if one or more fields was parsed</returns>
        public static bool ParseObject(object obj, string cvsPath)
        {
            var data = CSVParser.LoadFromPath(cvsPath);
            int rowIndex = 0;
            return ParseObject(obj, data, ref rowIndex);
        }

        /// <returns><see langword="true"/> if one or more fields was parsed</returns>
        public static bool ParseObject(object obj, in List<List<string>> data, ref int rowIndex)
        {
            IFieldParser parser;
            bool fieldResult;
            bool result = false;
            var objectType = obj.GetType();
            Logger.Log($"Start parsing of {objectType.Name} process...");

            var fields = objectType.GetFieldsWithCsvAttribute();
            Logger.Log($"Found {fields.Count()} fields to parse");

            foreach(var field in fields)
            {
                try
                {
                    parser = ParserContainer.GetParser(field);
                    fieldResult = parser.ParseField(obj, field, in data, ref rowIndex);
                    result |= fieldResult;
                    Logger.Log($"Parsed {field.Name} with result: {fieldResult}");
                }
                catch(Exception e)
                {
                    Logger.LogError(e.Message);
                }
            }

            Logger.Log($"Finish parsing of {objectType.Name} process...");
            return result;
        }
    }
}

