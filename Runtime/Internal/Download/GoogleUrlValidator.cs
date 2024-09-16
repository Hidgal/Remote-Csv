namespace RemoteCsv.Internal.Download
{
    public static class GoogleUrlValidator
    {
        public static string ValidateUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            if (!url.Contains("edit?gid="))
                return null;

            int idStartIndex = url.IndexOf("/d/") + 3;
            int idEndIndex = url.IndexOf("/edit?");
            string spreadsheetId = url.Substring(idStartIndex, idEndIndex - idStartIndex);

            int gidStartIndex = url.IndexOf("gid=") + 4;
            string gid = url.Substring(gidStartIndex);

            return $"https://docs.google.com/spreadsheets/d/{spreadsheetId}/export?format=csv&gid={gid}";
        }
    }
}
