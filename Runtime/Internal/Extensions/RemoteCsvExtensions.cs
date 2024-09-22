using RemoteCsv.Settings;
using UnityEngine;
using System.IO;

namespace RemoteCsv.Internal.Extensions
{
    public static class RemoteCsvExtensions
    {
        public static string GetAssetPath(this IRemoteCsvData remoteData)
        {
            var filePath = GetFilePath(remoteData);
            var assetsIndex = filePath.IndexOf("Assets");

            return filePath.Substring(assetsIndex);
        }

        public static string GetFilePath(this IRemoteCsvData remoteData)
        {
            return Path.Combine(Application.dataPath, RemoteCsvSettingsAsset.Instance.Settings.FolderPath, remoteData.FileName + remoteData.Extension);
        }
    }
}

