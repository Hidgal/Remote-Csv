using System.IO;
using UnityEngine;

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
            return Path.Combine(Application.dataPath, RemoteScriptablesList.Instance.DownloadFolderPath, remoteData.FileName + remoteData.Extension);
        }
    }
}

