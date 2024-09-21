using Logger = RemoteCsv.Internal.Logger;
using System.Collections.Generic;
using RemoteCsv.Settings;
using UnityEngine;

namespace RemoteCsv.Internal.Utility
{
    public static class RemoteCsvDataFinder
    {
        public static IRemoteCsvData[] GetDataFromScriptables(params ScriptableObject[] scriptables)
        {
            if (RemoteCsvSettingsAsset.Instance)
            {
                List<IRemoteCsvData> dataList = new();
                foreach (var scriptable in scriptables)
                {
                    if (!scriptable) continue;

                    var data = RemoteCsvSettingsAsset.Instance.GetDataByScriptable(scriptable);

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
