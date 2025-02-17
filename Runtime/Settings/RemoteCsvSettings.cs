using System.IO;
using UnityEngine;

namespace RemoteCsv.Settings
{
    [System.Serializable]
    public class RemoteCsvSettings
    {
        [SerializeField]
        private bool _saveCsvAssetsInEditor = false;
        [SerializeField]
        private bool _saveCsvAssetsInBuilds = true;

        public bool SaveAssetsAfterLoad => Application.isEditor ? _saveCsvAssetsInEditor : _saveCsvAssetsInBuilds;
        public string FolderPath => Path.Combine(Application.persistentDataPath, "RemoteCsv");
    }
}
