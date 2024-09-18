using System.Linq;
using UnityEngine;

namespace RemoteCsv.Internal
{
    [CreateAssetMenu(menuName = "Remote Csv/Scriptables List", fileName = DEFAULT_LIST_NAME)]
    public class RemoteScriptablesList : ScriptableObject
    {
        public const string DEFAULT_LIST_NAME = "Remote Scriptables List";

        private static RemoteScriptablesList _instance;
        public static RemoteScriptablesList Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.Load<RemoteScriptablesList>(DEFAULT_LIST_NAME);

                return _instance;
            }
        }

        [SerializeField]
        private string _downloadFolderPath = "RemoteCsv";
        [SerializeField]
        private RemoteCsvData[] _data;

        public IRemoteCsvData[] Data => _data;
        public string DownloadFolderPath => _downloadFolderPath;

        public bool HasData(ScriptableObject scriptable) => GetDataByScriptable(scriptable) != null;
        public IRemoteCsvData GetDataByScriptable(ScriptableObject scriptable)
        {
            if(!scriptable) return null;
            return _data.FirstOrDefault(data => data.TargetScriptable == scriptable);
        }

#if UNITY_EDITOR
        public void CreateDataArray() => _data = new RemoteCsvData[0];

        public bool TryAddData(ScriptableObject scriptable)
        {
            if (!HasData(scriptable))
            {
                var tempArray = new RemoteCsvData[_data.Length + 1];
                _data.CopyTo(tempArray, 0);
                tempArray[^1] = new(scriptable);
                _data = tempArray;

                UnityEditor.EditorUtility.SetDirty(this);
                return true;
            }

            return false;
        }

        public void RemoveNullRefs()
        {
            _data = _data.Where(data => data.TargetScriptable).ToArray();
            UnityEditor.EditorUtility.SetDirty(this);
        } 
#endif
    }
}

