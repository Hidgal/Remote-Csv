using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor; 
#endif

namespace RemoteCsv.Internal
{
    [System.Serializable]
    public class RemoteCsvData : IRemoteCsvData
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
    {
        private const string FILE_EXTENSION = ".csv";

#if UNITY_EDITOR
        //Name for better view in inspector
        [HideInInspector]
        public string Name;

        [HideInInspector]
        [SerializeField]
        private Object _dataAsset;
#endif

        [SerializeField]
        private ScriptableObject _targetScriptable;
        [SerializeField]
        private string _url;

        [HideInInspector]
        [SerializeField]
        private string _hash;
        [HideInInspector]
        [SerializeField]
        private string _fileName;

        public ScriptableObject TargetScriptable => _targetScriptable;
        public string FileName => _fileName;
        public string Url => _url;
        public string Extension => FILE_EXTENSION;
        public string Hash => _hash;

        public RemoteCsvData(ScriptableObject targetScriptable)
        {
            _targetScriptable = targetScriptable;
            UpdateFileName();
        }

        public void UpdateHash(string hash)
        {
            _hash = hash;
        }

#if UNITY_EDITOR
        public void OnAfterDeserialize() { }
        public void OnBeforeSerialize()
        {
            UpdateFilePath();
        }

        private void UpdateFilePath()
        {
            UpdateFileName();
        }

        private void UpdateFileName()
        {
            if (_dataAsset)
            {
                _fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(_dataAsset));
            }
            else
            {
                if (string.IsNullOrEmpty(_fileName))
                {
                    if (_targetScriptable)
                    {
                        var assetPath = AssetDatabase.GetAssetPath(_targetScriptable);
                        _fileName = Path.GetFileNameWithoutExtension(assetPath);
                    }
                    else
                        _fileName = "NoName";
                }
            }

            Name = ObjectNames.NicifyVariableName(_fileName);
        }
#endif
    }
}
