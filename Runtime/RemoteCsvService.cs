using System.Threading;
using RemoteCsv.Internal.Service;
using RemoteCsv.Internal.Utility;
using RemoteCsv.Settings;
using UnityEngine;

namespace RemoteCsv
{
    public static class RemoteCsvService
    {
        public static IRemoteCsvService LoadAndParse(params ScriptableObject[] scriptables)
        {
            var settings = RemoteCsvSettingsUtility.GetSettingsFromAsset();
            if (settings == null) return null;

            var remotes = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);

            return LoadAndParse(settings, remotes);
        }
        public static IRemoteCsvService LoadAndParse(RemoteCsvSettings settings, params ScriptableObject[] scriptables)
        {
            var remotes = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);
            return LoadAndParse(settings, remotes);
        }
        public static IRemoteCsvService LoadAndParse(params IRemoteCsvData[] remotes)
        {
            var settings = RemoteCsvSettingsUtility.GetSettingsFromAsset();
            if (settings == null) return null;

            return LoadAndParse(settings, remotes);
        }
        public static IRemoteCsvService LoadAndParse(RemoteCsvSettings settings, params IRemoteCsvData[] remotes)
        {
            return new AsyncRemoteCsvService(Application.exitCancellationToken, settings, remotes);
        }

        public static IRemoteCsvService LoadAndParse(CancellationToken token, params ScriptableObject[] scriptables)
        {
            var settings = RemoteCsvSettingsUtility.GetSettingsFromAsset();
            if (settings == null) return null;

            var remotes = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);
            return LoadAndParse(token, settings, remotes);
        }
        public static IRemoteCsvService LoadAndParse(CancellationToken token, RemoteCsvSettings settings, params ScriptableObject[] scriptables)
        {
            var remotes = RemoteCsvDataFinder.GetDataFromScriptables(scriptables);
            return LoadAndParse(token, settings, remotes);
        }
        public static IRemoteCsvService LoadAndParse(CancellationToken token, params IRemoteCsvData[] remotes)
        {
            var settings = RemoteCsvSettingsUtility.GetSettingsFromAsset();
            if (settings == null) return null;

            return LoadAndParse(token ,settings, remotes);
        }
        public static IRemoteCsvService LoadAndParse(CancellationToken token, RemoteCsvSettings setting, params IRemoteCsvData[] remotes)
        {
            return new AsyncRemoteCsvService(token, setting, remotes);
        }
    }
}
