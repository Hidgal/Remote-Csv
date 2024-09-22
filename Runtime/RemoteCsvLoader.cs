using RemoteCsv.Internal.Download.CoroutineLoader;
using UnityEngine;

#if UNITY_EDITOR
using RemoteCsv.Internal.Download.EditorCoroutineLoader;
using RemoteCsv.Internal.Utility;
using RemoteCsv.Settings;
#endif

#if UNITASK_INSTALLED
using RemoteCsv.Internal.Download.UniTaskLoader;
#endif

namespace RemoteCsv
{
    public class RemoteCsvLoader
    {
        public static IDownloadService GetLoader(MonoBehaviour coroutineRunner, params ScriptableObject[] scriptables)
        {
            var settings = RemoteCsvSettingsUtility.GetSettingsFromAsset();
            if (settings == null) return null;

            var data = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);
            return GetLoader(settings, coroutineRunner, data);
        }
        public static IDownloadService GetLoader(RemoteCsvSettings settings, MonoBehaviour coroutineRunner, params ScriptableObject[] scriptables)
        {
            var data = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);
            return GetLoader(settings, coroutineRunner, data);
        }
        public static IDownloadService GetLoader(RemoteCsvSettings settings, MonoBehaviour coroutineRunner, params IRemoteCsvData[] dataArray)
        {
#if UNITY_EDITOR
            if(!Application.isPlaying)
                return new EditorCoroutineDownloadService(settings, dataArray);
#endif

            return new CoroutineDownloadService(settings, coroutineRunner, dataArray);
        }

        public static IDownloadService GetLoader(params ScriptableObject[] scriptables)
        {
            var settings = RemoteCsvSettingsUtility.GetSettingsFromAsset();
            if (settings == null) return null;

            var data = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);
            return GetLoader(settings, data);
        }
        public static IDownloadService GetLoader(RemoteCsvSettings settings, params ScriptableObject[] scriptables)
        {
            var data = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);
            return GetLoader(settings, data);
        }
        public static IDownloadService GetLoader(RemoteCsvSettings settings, params IRemoteCsvData[] dataArray)
        {
#if UNITASK_INSTALLED
            return new UniTaskDownloadService(settings, dataArray);
#endif
#pragma warning disable CS0162

#if UNITY_EDITOR
            return new EditorCoroutineDownloadService(settings, dataArray);
#endif

            return null;

#pragma warning restore CS0162
        }
    }
}
