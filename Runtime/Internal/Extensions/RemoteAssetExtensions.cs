using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using yutokun;

namespace RemoteCsv.Internal.Extensions
{
    public static class RemoteAssetExtensions
    {
        public static string GetAssetPath(this IRemoteCsvData remoteData)
        {
            var filePath = GetFilePath(remoteData);
            var assetsIndex = filePath.IndexOf("Assets");

            return filePath.Substring(assetsIndex).Replace("Assets", string.Empty);
        }

        public static string GetFilePath(this IRemoteCsvData remoteData)
        {
            return Path.Combine(Application.streamingAssetsPath, remoteData.FileName + remoteData.Extension);
        }

        public static string ReadAllText(this IRemoteCsvData remoteData)
        {
            if (string.IsNullOrEmpty(remoteData.FileName))
                return string.Empty;

            if (string.IsNullOrEmpty(remoteData.Extension))
                return string.Empty;

            var filePath = GetFilePath(remoteData);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return string.Empty;
        }
        public static async UniTask<string> ReadAllTextAsync(this IRemoteCsvData remoteData, CancellationToken token)
        {
            if (string.IsNullOrEmpty(remoteData.FileName))
                return string.Empty;

            if (string.IsNullOrEmpty(remoteData.Extension))
                return string.Empty;

            var filePath = GetFilePath(remoteData);
            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath, token).AsUniTask();
            }

            return string.Empty;
        }


        public static List<List<string>> ReadCsv(this IRemoteCsvData remoteData)
        {
            var text = ReadAllText(remoteData);
            return CSVParser.LoadFromString(text);
        }
        public static async UniTask<List<List<string>>> ReadCsvAsync(this IRemoteCsvData remoteData, CancellationToken token)
        {
            var text = await ReadAllTextAsync(remoteData, token);

            return CSVParser.LoadFromString(text);
        }
    }
}

