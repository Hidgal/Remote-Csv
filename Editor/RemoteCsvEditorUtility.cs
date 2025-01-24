using Logger = RemoteCsv.Internal.Logger;
using RemoteCsv.Settings;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;

namespace RemoteCsv.Editor
{
    public static class RemoteCsvEditorUtility
    {
        private static IRemoteCsvService _service;

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

            _service?.Dispose();
            _service = RemoteCsvService.LoadAndParse(Application.exitCancellationToken, RemoteCsvSettingsAsset.Instance.Data);
            _service.Start();
        }

        [MenuItem("CONTEXT/ScriptableObject/Parse From Csv")]
        public static void FetchData(MenuCommand command)
        {
            var type = command.context.GetType();
            if (RemoteCsvTypeUtility.IsAvailableType(type))
            {
                var service = RemoteCsvService.LoadAndParse(Application.exitCancellationToken, command.context as ScriptableObject);
                service.Start();
            }
            else
            {
                Logger.LogError($"No data to parse from CSV in {command.context.name} (type: {type.Name})");
            }
        }

        [MenuItem("Tools/Remote Csv/Open Downloads Folder", priority = 10)]
        public static void OpenDownloadFolder()
        {
            if (RemoteCsvSettingsAsset.Instance == null)
            {
                Logger.LogError($"Can`t open download folder - there`s no Remote Csv Settings Asset!");
                return;
            }

            var settings = RemoteCsvSettingsAsset.Instance.Settings;

            if (Directory.Exists(settings.FolderPath) == false)
            {
                Directory.CreateDirectory(settings.FolderPath);
            }

            Process.Start(settings.FolderPath);
        }
    }
}
