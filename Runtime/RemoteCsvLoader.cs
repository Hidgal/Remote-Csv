using System.Collections.Generic;
using System.Threading;
using RemoteCsv.Internal;
using RemoteCsv.Internal.Download.UniTaskLoader;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv
{
    public class RemoteCsvLoader
    {
        public static IDownloadService Load(CancellationToken cancellationToken, params ScriptableObject[] scriptables)
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

                return Load(cancellationToken, dataList.ToArray());
            }
            else
            {
                throw new MissingReferenceException("Remote Scriptables List asset was not found in Resources folder!");
            }
        }

        public static IDownloadService Load(CancellationToken cancellationToken, params IRemoteCsvData[] dataArray)
        {
#if UNITASK_INSTALLED
            return new UniTaskDownloadService(cancellationToken, dataArray); 
#endif
        }
    }
}
