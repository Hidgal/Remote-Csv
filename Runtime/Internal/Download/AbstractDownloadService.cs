using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RemoteCsv.Internal.Download
{
    public abstract class AbstractDownloadService : IDownloadService
    {
        public event Action OnLoadFinish;

        protected readonly bool _forceRefreshLocalData;
        protected readonly IReadOnlyList<IRemoteCsvData> _remotes;

        protected CancellationTokenSource _cts;
        protected bool _isCompleted;

        private DateTime _startDate;

        public bool IsFinished => _isCompleted;

        public AbstractDownloadService(CancellationToken token = default, params IRemoteCsvData[] remotes)
        {
            _forceRefreshLocalData = UnityEngine.Application.isEditor;

            _remotes = remotes;
            token.Register(OnRootTokenCancel);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            StartLoading();
        }

        public virtual void Dispose()
        {
            if(_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        protected abstract void StartLoadingInternal();

        protected virtual void OnLoadingFinished()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.AssetDatabase.SaveAssets(); 
#endif

            var loadTime = DateTime.Now - _startDate;

            var resultLogBuilder = new StringBuilder();
            resultLogBuilder.AppendLine($"Loading finished!");
            resultLogBuilder.AppendLine($"Total load time: {loadTime:mm\\:ss\\:ff}");
            resultLogBuilder.AppendLine($"Remotes count: {_remotes.Count}");

            Logger.Log(resultLogBuilder.ToString());

            _isCompleted = true;
            OnLoadFinish?.Invoke();
        }

        private void StartLoading()
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

            _startDate = DateTime.Now;
            Logger.Log($"Loading started...");

            StartLoadingInternal();
        }

        private void OnRootTokenCancel()
        {
            Dispose();
            Logger.LogError("Load operation was canceled");
        }
    }
}

