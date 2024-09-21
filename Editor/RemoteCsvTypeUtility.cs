using System;
using System.Collections.Generic;
using RemoteCsv.Internal.Extensions;
using System.Linq;

namespace RemoteCsv.Editor
{
    public static class RemoteCsvTypeUtility
    {
        public static bool IsAvailableType(Type type) => type.GetFieldsWithCsvAttribute().Count() > 0;

        public static IEnumerable<Type> GetAvailableTypes()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (IsAvailableType(type))
                        yield return type;
                }
            }
        }
    }
}

