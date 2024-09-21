using System;

namespace RemoteCsv
{
    /// <summary>
    /// Load CSV file and then parse it to the target scriptable
    /// </summary>
    public interface IRemoteCsvService : IDisposable
    {
        event Action OnFinished;
        bool IsFinished { get; }

        void Start(); 
    }
}
