using UnityEngine;
using RemoteCsv.Internal.Extensions;


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
        private const string _extension = ".csv";

#if UNITY_EDITOR
        //Name for better view in inspector
        [HideInInspector]
        [SerializeField]
        public string Name;

        [HideInInspector]
        [SerializeField]
        private Object _dataAsset;
#endif

        [SerializeField]
        private ScriptableObject _targetScriptable;
        [SerializeField]
        private bool _autoParseAfterLoad;
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
        public string Extension => _extension;
        public string Hash => _hash;
        public bool AutoParseAfterLoad => _autoParseAfterLoad;

#if UNITY_EDITOR
        public RemoteCsvData()
        {
            _autoParseAfterLoad = true;
        }
        public RemoteCsvData(ScriptableObject targetScriptable)
        {
            _targetScriptable = targetScriptable;
            _autoParseAfterLoad = true;
            UpdateFileName();
        }

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

        public void OnDataLoaded()
        {
#if UNITY_EDITOR
            _dataAsset = AssetDatabase.LoadAssetAtPath<Object>(this.GetAssetPath());

            if (_dataAsset)
            {
                var data = File.ReadAllText(this.GetFilePath());
                _hash = FileExtensions.GetHash(data);
            }

            if (_targetScriptable)
            {
                EditorUtility.SetDirty(_targetScriptable); 
            }
#endif
        }
    }
}
