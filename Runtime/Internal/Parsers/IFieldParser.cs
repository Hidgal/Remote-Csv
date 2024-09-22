using System;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public interface IFieldParser
    {
        /// <summary>
        /// Method for parse field value and paste it to the actual field
        /// </summary>
        bool ParseField(object obj, FromCsvAttribute attribute, FieldInfo field, in List<List<string>> data, ref int lastRowIndex);

        /// <summary>
        /// Method for parse value of provided type. Used for parsing Array values and other IEnumerable.
        /// </summary>
        bool ParseValue(FromCsvAttribute attribute, in List<List<string>> data, ref int lastRowIndex, out object value, Type type = null);
    }
}

