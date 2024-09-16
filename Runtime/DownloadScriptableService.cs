using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using RemoteCsv.Internal;
using RemoteCsv.Internal.Download;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv
{
    public class DownloadScriptableService : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly bool _forceRefreshLocalData;
        private readonly IReadOnlyList<IRemoteCsvData> _remotes;

        public DownloadScriptableService(CancellationToken token = default, params IRemoteCsvData[] remotes)
        {
            _forceRefreshLocalData = UnityEngine.Application.isEditor;

            _remotes = remotes;
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            _cts.Token.Register(LogCancellation);

            LoadData().Forget();
        }

        public static void Load(params ScriptableObject[] scriptables)
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

                Load(dataList.ToArray());
            }
            else
            {
                throw new MissingReferenceException("Remote Scriptables List asset was not found in Resources folder!");
            }
        }
        public static void Load(params IRemoteCsvData[] dataArray)
        {
            new DownloadScriptableService(Application.exitCancellationToken, dataArray);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        private async UniTaskVoid LoadData()
        {
            if (_remotes == null)
            {
                Logger.LogError("Can`t start data loading - remotes list is null");
                return;
            }

            if (_remotes.Count == 0)
            {
                Logger.Log("There`s no remotes to load");
                return;
            }

            var startDate = DateTime.Now;
            Logger.Log($"Loading started...");

            var loadOperations = new UniTask[_remotes.Count];
            for (int i = 0; i < _remotes.Count; i++)
            {
                if (_remotes[i] == null) continue;

                var operation = new AsyncDownloadOperation(i, _remotes[i], _cts.Token, _forceRefreshLocalData);
                loadOperations[i] = operation.LoadData();
            }

            await UniTask.WhenAll(loadOperations);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.AssetDatabase.SaveAssets();
#endif

            var loadTime = DateTime.Now - startDate;

            var resultLogBuilder = new StringBuilder();
            resultLogBuilder.AppendLine($"Loading finished!");
            resultLogBuilder.AppendLine($"Total load time: {loadTime:mm\\:ss\\:ff}");
            resultLogBuilder.AppendLine($"Remotes count: {_remotes.Count}");

            Logger.Log(resultLogBuilder.ToString());
        }

        private void LogCancellation()
        {
            Logger.LogError("operation was canceled");
        }
    }
}
