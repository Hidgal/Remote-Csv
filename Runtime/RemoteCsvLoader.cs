using System.Collections.Generic;
using System.Threading;
using RemoteCsv.Internal;
using RemoteCsv.Internal.Download.CoroutineLoader;
using RemoteCsv.Internal.Download.EditorCoroutineLoader;
using RemoteCsv.Internal.Download.UniTaskLoader;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv
{
    public class RemoteCsvLoader
    {
        public static IDownloadService Load(CancellationToken cancellationToken, params ScriptableObject[] scriptables)
        {
            var data = GetDataFromScriptables(scriptables);
            return Load(cancellationToken, data);
        }
        public static IDownloadService Load(CancellationToken cancellationToken, params IRemoteCsvData[] dataArray)
        {
#if UNITASK_INSTALLED
            return new UniTaskDownloadService(cancellationToken, dataArray);
#endif
#pragma warning disable CS0162

#if UNITY_EDITOR
            return new EditorCoroutineDownloadService(cancellationToken, dataArray);
#endif

            return null;

#pragma warning restore CS0162
        }

        public static IDownloadService Load(MonoBehaviour coroutineRunner, params ScriptableObject[] scriptables)
        {
            var data = GetDataFromScriptables(scriptables);
            return Load(coroutineRunner, data);
        }
        public static IDownloadService Load(MonoBehaviour coroutineRunner, params IRemoteCsvData[] dataArray)
        {
            return new CoroutineDownloadService(coroutineRunner, dataArray);
        }

        private static IRemoteCsvData[] GetDataFromScriptables(ScriptableObject[] scriptables)
        {
            if (RemoteScriptablesList.Instance)
            {
                List<IRemoteCsvData> dataList = new();
                foreach (var scriptable in scriptables)
                {
                    if (!scriptable) continue;

                    var data = RemoteScriptablesList.Instance.GetDataByScriptable(scriptable);

                    if (data != null)
                        dataList.Add(data);
                    else
                        Logger.LogError($"No remote data for {scriptable.name}!");
                }

                return dataList.ToArray();
            }
            else
            {
                throw new MissingReferenceException("Remote Scriptables List asset was not found in Resources folder!");
            }
        }
    }
}
