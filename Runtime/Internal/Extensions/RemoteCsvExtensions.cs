using RemoteCsv.Settings;
using System.IO;

namespace RemoteCsv.Internal.Extensions
{
    public static class RemoteCsvExtensions
    {
        public static string GetFilePath(this IRemoteCsvData remoteData)
        {
            return Path.Combine(RemoteCsvSettingsAsset.Instance.Settings.FolderPath, remoteData.FileName + remoteData.Extension);
        }
    }
}

