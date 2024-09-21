using System.Linq;
using RemoteCsv.Internal;
using UnityEngine;

namespace RemoteCsv.Settings
{
    [CreateAssetMenu(menuName = "Remote Csv/Settings Asset", fileName = DEFAULT_LIST_NAME)]
    public class RemoteCsvSettingsAsset : ScriptableObject
    {
        public const string DEFAULT_LIST_NAME = "Remote Csv Settings";

        private static RemoteCsvSettingsAsset _instance;
        public static RemoteCsvSettingsAsset Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.Load<RemoteCsvSettingsAsset>(DEFAULT_LIST_NAME);

                return _instance;
            }
        }

        [SerializeField]
        private RemoteCsvSettings _settings = new();
        [SerializeField]
        private RemoteCsvData[] _data = new RemoteCsvData[0];

        public IRemoteCsvData[] Data => _data;
        public RemoteCsvSettings Settings => _settings;

#if UNITY_EDITOR
        /// <summary>
        /// Available in editor only!
        /// </summary>
        public RemoteCsvData[] InternalDataArray
        {
            get => _data;
            set => _data = value;
        } 
#endif

        public bool HasData(ScriptableObject scriptable) => GetDataByScriptable(scriptable) != null;
        public IRemoteCsvData GetDataByScriptable(ScriptableObject scriptable)
        {
            if(!scriptable) return null;
            if (_data.Length == 0) return null;

            return _data.FirstOrDefault(data => data.TargetScriptable == scriptable);
        }
    }
}

