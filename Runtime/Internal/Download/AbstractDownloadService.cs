using System;
using System.Text;
using System.Threading;
using RemoteCsv.Settings;

namespace RemoteCsv.Internal.Download
{
    public abstract class AbstractDownloadService : IDownloadService
    {
        public event Action OnLoadFinish;

        protected readonly IRemoteCsvData[] _remotes;
        protected readonly RemoteCsvSettings _settings;

        protected IDownloadOperation[] _operations;
        protected CancellationTokenSource _cts;
        protected DownloadResult[] _result;
        protected Action _onCompleteCallback;
        protected bool _isFinished;

        private DateTime _startDate;

        /// <summary>
        /// Download result data for each remote. Return null if not finished yet.
        /// </summary>
        public DownloadResult[] Result => _result;
        public IRemoteCsvData[] Remotes => _remotes;
        public bool IsFinished => _isFinished;
        public bool IsSuccessed => _result != null;

        public AbstractDownloadService(RemoteCsvSettings settings, params IRemoteCsvData[] remotes)
        {
            _remotes = remotes;
            _settings = settings;
            _operations = new IDownloadOperation[remotes.Length];
        }

        public virtual void Dispose()
        {
            if(_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            Finish();
        }

        public void StartLoading(CancellationToken token, Action onCompleteCallback = null)
        {
            if (_cts != null)
            {
                Logger.LogWarning("Can`t start data loading - loading is already started");
                return;
            }

            if (_remotes == null)
            {
                Logger.LogError("Can`t start data loading - remotes list is null");
                return;
            }

            if (_remotes.Length == 0)
            {
                Logger.Log("There`s no remotes to load");
                return;
            }

            _startDate = DateTime.Now;
            token.Register(OnRootTokenCancel);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            _onCompleteCallback = onCompleteCallback;

            Logger.Log($"Loading started...");

            StartLoadingInternal();
        }

        protected abstract void StartLoadingInternal();

        protected virtual void OnLoadingFinished()
        {
#if UNITY_EDITOR
            if (_settings.SaveAssetsAfterLoad)
            {
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }
#endif

            _result = new DownloadResult[_remotes.Length];
            for (int i = 0; i < _operations.Length; i++)
            {
                if (_operations[i] == null)
                {
                    _result[i] = new();
                    continue;
                }

                _result[i] = _operations[i].Result;
            }

            var loadTime = DateTime.Now - _startDate;

            var resultLogBuilder = new StringBuilder();
            resultLogBuilder.AppendLine($"Loading finished!");
            resultLogBuilder.AppendLine($"Total load time: {loadTime:mm\\:ss\\:ff}");
            resultLogBuilder.AppendLine($"Remotes count: {_remotes.Length}");

            Logger.Log(resultLogBuilder.ToString());

            Finish();
        }

        private void OnRootTokenCancel()
        {
            Dispose();
            Logger.LogError("Load operation was canceled");
        }

        private void Finish()
        {
            _isFinished = true;
            OnLoadFinish?.Invoke();

            _onCompleteCallback?.Invoke();
        }
    }
}

