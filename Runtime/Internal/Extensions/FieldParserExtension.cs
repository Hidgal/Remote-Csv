using RemoteCsv.Internal.Parsers;
using UnityEngine;

namespace RemoteCsv.Internal.Extensions
{
    public static class FieldParserExtension
    {
        public static int GetActualRowIndex(this IFieldParser parser, int rowIndex, ref int lastRowIndex)
        {
            if (rowIndex <= 0)
            {
                rowIndex = lastRowIndex;
                lastRowIndex++;
            }
            else
            {
                lastRowIndex = Mathf.Max(lastRowIndex, rowIndex);
            }

            return rowIndex;
        }
    }
}

