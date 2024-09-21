using RemoteCsv.Settings;
using UnityEditor;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv.Editor
{
    public static class RemoteCsvEditorUtility
    {
        private static IDownloadService _downloadService;

        [MenuItem("Tools/Remote Csv/Refresh All")]
        public static void RefreshAll()
        {
            if (Application.isPlaying)
            {
                Logger.LogError("Can`t load data while in playmode!");
                return;
            }

            Logger.Log($"Starting remotes refresh...");

            SettngsAssetUtility.TryCreateListAsset(true);

            Logger.Log($"Found {RemoteCsvSettingsAsset.Instance.Data.Length} remote assets in project. Start loading data...");

            _downloadService?.Dispose();
            _downloadService = RemoteCsvLoader.Load(Application.exitCancellationToken, RemoteCsvSettingsAsset.Instance.Data);
        }

        [MenuItem("CONTEXT/ScriptableObject/Parse From Csv")]
        public static void FetchData(MenuCommand command)
        {
            var type = command.context.GetType();
            if (RemoteCsvTypeUtility.IsAvailableType(type))
            {
                RemoteCsvLoader.Load(Application.exitCancellationToken, command.context as ScriptableObject);
            }
            else
            {
                Logger.LogError($"No data to parse from CSV in {command.context.name} (type: {type.Name})");
            }
        }
    }
}
