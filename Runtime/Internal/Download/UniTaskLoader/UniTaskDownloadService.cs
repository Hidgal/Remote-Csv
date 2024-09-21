#if UNITASK_INSTALLED
using Cysharp.Threading.Tasks;
using RemoteCsv.Settings;

namespace RemoteCsv.Internal.Download.UniTaskLoader
{
    public class UniTaskDownloadService : AbstractDownloadService
    {
        public UniTaskDownloadService(RemoteCsvSettings settings, params IRemoteCsvData[] remotes) : base(settings, remotes) { }

        protected override void StartLoadingInternal()
        {
            LoadData().Forget();
        }

        private async UniTaskVoid LoadData()
        {
            var tasks = new UniTask[_remotes.Length];
            for (int i = 0; i < _remotes.Length; i++)
            {
                if (_remotes[i] == null) continue;

                var operation = new UniTaskDownloadOperation(_settings, i, _remotes[i], _cts.Token);
                _operations[i] = operation;
                tasks[i] = operation.LoadData();
            }

            await UniTask.WhenAll(tasks);

            OnLoadingFinished();
        }
    }
}
#endif