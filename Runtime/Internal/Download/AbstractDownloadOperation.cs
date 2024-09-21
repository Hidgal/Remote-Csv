using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RemoteCsv.Internal.Extensions;
using RemoteCsv.Internal.Parsers;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download
{
    public abstract class AbstractDownloadOperation
    {
        protected readonly CancellationToken _token;
        protected readonly IRemoteCsvData _remoteData;
        protected readonly bool _forceParseData;
        protected readonly int _index;

        protected UnityWebRequest _request;
        protected string _currentHash;
        protected string _resultLog;
        protected string _filePath;
        protected string _url;

        protected string _name => _remoteData.FileName;
        protected string _dataHash => _remoteData.Hash;
        protected string _fileNameWithExtension => _remoteData.Extension;

        public AbstractDownloadOperation(int index, IRemoteCsvData remoteScriptable, CancellationToken token, bool forceParseData)
        {
            _index = index;
            _token = token;
            _remoteData = remoteScriptable;
            _forceParseData = forceParseData;

            if(_remoteData != null)
                _url = GoogleUrlValidator.ValidateUrl(_remoteData.Url);
        }

        protected void FinishLoading()
        {
            if (_remoteData.AutoParseAfterLoad || _forceParseData)
            {
                RemoteCsvParser.ParseObject(_remoteData.TargetScriptable, _remoteData.GetFilePath());
            }

            _request.Dispose();
            _remoteData.OnDataLoaded();

            LogResult();
        }

        protected bool IsHashChanged()
        {
            if (!string.IsNullOrEmpty(_currentHash))
            {
                var hashSum = FileExtensions.GetHash(_request.downloadHandler.data);

                //No need to import asset if hashes are equal
                if (_currentHash == hashSum)
                {
                    _resultLog = "is already up to date";
                    return false;
                }
            }

            return true;
        }

        protected void TryCreateDirectory()
        {
            var directoryPath = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        protected void ImportCsvAsset()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(_remoteData.GetAssetPath());
#endif
        }

        protected string GetCurrentHash()
        {
            if (_forceParseData)
            {
                if (File.Exists(_filePath))
                {
                    return FileExtensions.GetHash(File.ReadAllBytes(_filePath));
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return _dataHash;
            }
        }

        protected bool IsRequestValid()
        {
            if (_request.result == UnityWebRequest.Result.ConnectionError || _request.result == UnityWebRequest.Result.ProtocolError)
            {
                Logger.LogError($"[{_index}] {_name} downloading error: {_request.error}. \n URL: {_url}");
                return false;
            }

            if (_request.downloadHandler.text.Contains(DownloadServiceConsts.NonPublicUrlContent))
            {
                Logger.LogError($"[{_index}] {_name} provided URL isn`t public");
                return false;
            }

            return true;
        }

        protected bool IsInputDataValid()
        {
            if (_remoteData == null)
            {
                Logger.LogError($"Null refence remote for {_index}");
                return false;
            }

            if (!_remoteData.TargetScriptable)
            {
                Logger.LogError($"Null reference scriptable for {_index}");
                return false;
            }

            if (string.IsNullOrEmpty(_url))
            {
                Logger.LogError($"Invalid URL for [{_index}] {_name}");
                return false;
            }

            if (string.IsNullOrEmpty(_fileNameWithExtension))
            {
                Logger.LogError($"Invalid file path for [{_index}] {_name}");
                return false;
            }

            return true;
        }

        protected void LogResult()
        {
            var resultLogBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(_resultLog))
                _resultLog = string.IsNullOrEmpty(_currentHash) ? "downloaded" : "updated";

            resultLogBuilder.AppendLine($"[{_index}] {_name}: {_resultLog}");
            resultLogBuilder.AppendLine($"File Path: {_filePath}");

#if UNITY_EDITOR
            var size = FileExtensions.GetSizeString((ulong)new FileInfo(_filePath).Length);
            resultLogBuilder.AppendLine($"Download Size: {size}");
#endif

            Logger.Log(resultLogBuilder.ToString());
        }

        protected async Task UpdateFile()
        {
            _filePath = _remoteData.GetFilePath();
            _currentHash = GetCurrentHash();

            if (!IsHashChanged()) return;

            TryCreateDirectory();

            if (_request.downloadHandler.data != null)
                await File.WriteAllBytesAsync(_filePath, _request.downloadHandler.data, cancellationToken: _token);
            else
                await File.WriteAllTextAsync(_filePath, string.Empty, cancellationToken: _token);

            ImportCsvAsset();
        }
    }
}
