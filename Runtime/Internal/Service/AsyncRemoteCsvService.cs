using System.Threading;
using RemoteCsv.Settings;

namespace RemoteCsv.Internal.Service
{
    public class AsyncRemoteCsvService : AbstractRemoteCsvService
    {
        private readonly CancellationToken _token;

        public AsyncRemoteCsvService(CancellationToken token, RemoteCsvSettings settings, IRemoteCsvData[] remotes) : base(settings, remotes)
        {
            _token = token;
        }

        protected override IDownloadService GetDownloadService()
        {
            return RemoteCsvLoader.GetLoader(_settings, _remotes);
        }

        protected override void StartLoading()
        {
            _downloadService.StartLoading(_token);
        }
    }
}
