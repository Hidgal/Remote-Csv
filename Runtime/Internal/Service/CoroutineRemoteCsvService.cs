using RemoteCsv.Settings;
using UnityEngine;

namespace RemoteCsv.Internal.Service
{
    public class CoroutineRemoteCsvService : AbstractRemoteCsvService
    {
        private readonly MonoBehaviour _coroutineRunner;

        public CoroutineRemoteCsvService(MonoBehaviour coroutineRunner, RemoteCsvSettings settings, IRemoteCsvData[] remotes) : base(settings, remotes)
        {
            _coroutineRunner = coroutineRunner;
        }

        protected override IDownloadService GetDownloadService()
        {
            return RemoteCsvLoader.GetLoader(_settings, _coroutineRunner, _remotes);
        }

        protected override void StartLoading()
        {
            _downloadService.StartLoading(Application.exitCancellationToken);
        }
    }
}
