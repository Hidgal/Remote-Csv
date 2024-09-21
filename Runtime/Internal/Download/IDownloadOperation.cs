namespace RemoteCsv.Internal.Download
{
    public interface IDownloadOperation
    {
        DownloadResult Result { get; }
    }
}