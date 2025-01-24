namespace RemoteCsv
{
    public class DownloadResult
    {
        private readonly bool _isLoaded;
        private readonly byte[] _data;

        public bool IsLoaded => _isLoaded;
        public byte[] Data => _data;

        public DownloadResult()
        {
            _isLoaded = false;
            _data = new byte[0];
        }

        public DownloadResult(byte[] data)
        {
            _isLoaded = true;
            
            if(data == null)
                _data = new byte[0];
            else
            _data = data;
        }
    }
}

