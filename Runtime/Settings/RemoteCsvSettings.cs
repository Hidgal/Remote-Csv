using UnityEngine;

namespace RemoteCsv.Settings
{
    [System.Serializable]
    public class RemoteCsvSettings
    {
        private const string DEFAULT_SAVE_FOLDER_PATH = "RemoteCsv";

        [SerializeField]
        private bool _saveCsvAssetsAfterLoad = true;
        [SerializeField]
        private string _saveFolderPath = DEFAULT_SAVE_FOLDER_PATH;

        public bool SaveAssetsAfterLoad => _saveCsvAssetsAfterLoad;
        public string FolderPath => _saveFolderPath;
    }
}
