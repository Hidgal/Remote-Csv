namespace RemoteCsv.Internal
{
    public static class Logger
    {
        private const string _logPrefix = "Remote Csv: ";
        private const string _logErrorPrefix = "Remote Csv Error: ";

        public static void Log(string message)
        {
            UnityEngine.Debug.Log(_logPrefix + message);
        }

        public static void LogError(string message)
        {
            UnityEngine.Debug.LogError(_logErrorPrefix + message);
        }

        public static void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(_logPrefix + message);
        }
    }
}
