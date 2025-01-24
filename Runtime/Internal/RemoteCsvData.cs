using UnityEngine;

namespace RemoteCsv.Internal
{
    [System.Serializable]
    public class RemoteCsvData : IRemoteCsvData
    {
        private const string FILE_EXTENSION = ".csv";

#if UNITY_EDITOR
        //Name for better view in inspector
        [HideInInspector]
        public string Name;
#endif

        [SerializeField]
        private ScriptableObject _targetScriptable;
        [SerializeField]
        private string _url;

        public ScriptableObject TargetScriptable => _targetScriptable;
        public string FileName => GetFileName();
        public string Url => _url;
        public string Extension => FILE_EXTENSION;

        public RemoteCsvData(ScriptableObject targetScriptable)
        {
            _targetScriptable = targetScriptable;
        }

        public string GetFileName()
        {
            if(_targetScriptable == false) return string.Empty;

            return _targetScriptable.name.Replace(" ", "");
        }
    }
}
