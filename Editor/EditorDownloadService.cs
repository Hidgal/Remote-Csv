using RemoteCsv.Internal;
using UnityEditor;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv.Editor
{
    public static class EditorDownloadService
    {
        [MenuItem("Tools/Remote Csv/Refresh All")]
        public static void RefreshAll()
        {
            if (Application.isPlaying)
            {
                Logger.LogError("Can`t load data while in playmode!");
                return;
            }

            Logger.Log($"Starting remotes refresh...");

            RemotesScriptableListUtility.TryCreateListAsset(true);

            Logger.Log($"Found {RemoteScriptablesList.Instance.Data.Length} remote assets in project. Start loading data...");

            DownloadScriptableService.Load(RemoteScriptablesList.Instance.Data);
        }

        [MenuItem("CONTEXT/ScriptableObject/Parse From Csv")]
        public static void FetchData(MenuCommand command)
        {
            var type = command.context.GetType();
            if (RemoteTypesUtility.IsAvailableType(type))
            {
                DownloadScriptableService.Load(command.context as ScriptableObject);
            }
            else
            {
                Logger.LogError($"No data to parse from CSV in {command.context.name} (type: {type.Name})");
            }
        }
    }
}
