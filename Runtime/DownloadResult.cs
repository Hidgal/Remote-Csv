namespace RemoteCsv
{
    public class DownloadResult
    {
        private readonly bool _isLoaded;
        private readonly byte[] _data;
        private readonly string _hash;

        public bool IsLoaded => _isLoaded;
        public byte[] Data => _data;
        public string Hash => _hash;

        public DownloadResult()
        {
            _isLoaded = false;
            _data = new byte[0];
            _hash = string.Empty;
        }

        public DownloadResult(byte[] data, string hash)
        {
            _isLoaded = true;
            _hash = hash;
            
            if(data == null)
                _data = new byte[0];
            else
            _data = data;
        }
    }
}

