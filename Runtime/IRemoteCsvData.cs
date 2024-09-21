using UnityEngine;

namespace RemoteCsv
{
    public interface IRemoteCsvData
    {
        /// <summary>
        /// Scriptable to parse
        /// </summary>
        public ScriptableObject TargetScriptable { get; }

        /// <summary>
        /// File name without extension
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Csv downloading URL
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Path to loaded file
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// Hash sum for checking file update
        /// </summary>
        public string Hash { get; }

        public bool AutoParseAfterLoad { get; }

        void UpdateHash(); 
    }
}
