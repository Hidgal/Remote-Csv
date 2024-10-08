using RemoteCsv.Internal;
using RemoteCsv.Internal.Extensions;
using RemoteCsv.Internal.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using yutokun;

namespace RemoteCsv
{
    public class RemoteCsvParser
    {
        /// <returns><see langword="true"/> if one or more fields was parsed</returns>
        public static bool ParseObject(object obj, string cvsPath)
        {
            var data = CSVParser.LoadFromPath(cvsPath);
            int rowIndex = 0;
            return ParseObject(ref obj, data, ref rowIndex);
        }

        /// <returns><see langword="true"/> if one or more fields was parsed</returns>
        public static bool ParseObject(object obj, in List<List<string>> data)
        {
            int rowIndex = 0;
            return ParseObject(ref obj, in data, ref rowIndex);
        }

        /// <returns><see langword="true"/> if one or more fields was parsed</returns>
        public static bool ParseObject(object obj, byte[] data)
        {
            var dataString = System.Text.Encoding.Default.GetString(data);
            var dataList = CSVParser.LoadFromString(dataString);
            int rowIndex = 0;
            return ParseObject(ref obj, in dataList, ref rowIndex);
        }

        /// <returns><see langword="true"/> if one or more fields was parsed</returns>
        public static bool ParseObject(ref object obj, in List<List<string>> data, ref int rowIndex)
        {
            if (obj == null) return false;

            IFieldParser parser;
            bool fieldResult;
            FromCsvAttribute attribute;
            bool result = false;
            var objectType = obj.GetType();
            Logger.Log($"Start parsing of {objectType.Name} process...");

            var fields = objectType.GetFieldsWithCsvAttribute();
            Logger.Log($"Found {fields.Count()} fields to parse");

            foreach (var field in fields)
            {
                try
                {
                    attribute = field.GetCsvAttribute();
                    parser = ParserContainer.GetParser(field, attribute);
                    fieldResult = parser.ParseField(obj, attribute, field, in data, ref rowIndex);
                    result |= fieldResult;
                    Logger.Log($"Parsed field: {field.Name}, with result: {fieldResult}");
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                }
            }

            Logger.Log($"Finish parsing of {objectType.Name} process...");
            return result;
        }
    }
}

