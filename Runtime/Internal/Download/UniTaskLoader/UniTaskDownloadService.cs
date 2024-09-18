#if UNITASK_INSTALLED
using System.Threading;
using Cysharp.Threading.Tasks;

namespace RemoteCsv.Internal.Download.UniTaskLoader
{
    public class UniTaskDownloadService : AbstractDownloadService
    {
        public UniTaskDownloadService(CancellationToken token = default, params IRemoteCsvData[] remotes)
            : base(token, remotes) { }

        protected override void StartLoadingInternal()
        {
            LoadData().Forget();
        }

        private async UniTaskVoid LoadData()
        {
            var loadOperations = new UniTask[_remotes.Count];
            for (int i = 0; i < _remotes.Count; i++)
            {
                if (_remotes[i] == null) continue;

                var operation = new UniTaskDownloadOperation(i, _remotes[i], _cts.Token, _forceRefreshLocalData);
                loadOperations[i] = operation.LoadData();
            }

            await UniTask.WhenAll(loadOperations);

            OnLoadingFinished();
        }
    }
}
#endif