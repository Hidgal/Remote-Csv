using System;
using System.Threading;

namespace RemoteCsv
{
    public interface IDownloadService : IDisposable
    {
        event Action OnLoadFinish;

        bool IsFinished { get; }
        DownloadResult[] Result { get; }
        IRemoteCsvData[] Remotes { get; }
        bool IsSuccessed { get; }

        void StartLoading(CancellationToken token, Action onCompleteCallback = null);
    }
}

