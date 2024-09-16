namespace RemoteCsv.Internal.Extensions
{
    public static class DataString
    {
        public static string FormatForNumbers(in string dataString)
        {
            return dataString
                .Replace("%", "");
        }
    }
}

