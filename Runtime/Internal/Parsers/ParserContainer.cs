using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RemoteCsv.Internal.Parsers
{
    public static class ParserContainer
    {
        private static Type _enumerableType = typeof(IEnumerable);
        private static IFieldParser _classParser = new ClassParser();
        private static IFieldParser _arrayParser = new EnumerableParser();

        private static Dictionary<Type, IFieldParser> _defaultTypeParsers = new()
        {
            { typeof(int), new IntParser() },
            { typeof(float), new FloatParser() },
            { typeof(string), new StringParser() }
        };

        public static IFieldParser GetParser(FieldInfo field) => GetParser(field.FieldType);
        public static IFieldParser GetParser(Type type)
        {
            if (_defaultTypeParsers.TryGetValue(type, out var parser))
            {
                return parser;
            }

            if (_enumerableType.IsAssignableFrom(type))
            {
                if (type.IsArray)
                    return _arrayParser;
                else
                    throw new Exception("Lists and other generic collections are not supported. Use array instead.");
            }

            return _classParser;
        }
    }
}

