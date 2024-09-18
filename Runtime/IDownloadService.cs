using System;

namespace RemoteCsv
{
    public interface IDownloadService : IDisposable
    {
        event Action OnLoadFinish;
        bool IsFinished { get; }
    }
}

