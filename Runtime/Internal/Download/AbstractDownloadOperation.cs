using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RemoteCsv.Internal.Extensions;
using RemoteCsv.Settings;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download
{
    public abstract class AbstractDownloadOperation : IDownloadOperation
    {
        protected readonly RemoteCsvSettings _settings;
        protected readonly CancellationToken _token;
        protected readonly IRemoteCsvData _remoteData;
        protected readonly int _index;

        protected DownloadResult _result;
        protected UnityWebRequest _request;
        protected string _resultLog;
        protected string _filePath;
        protected string _url;

        protected string _name => _remoteData.FileName;
        protected string _dataHash => _remoteData.Hash;
        protected string _fileNameWithExtension => _remoteData.Extension;

        public DownloadResult Result => _result == null ? new() : _result;

        public AbstractDownloadOperation(RemoteCsvSettings settings, int index, IRemoteCsvData remoteScriptable, CancellationToken token)
        {
            _index = index;
            _token = token;
            _settings = settings;
            _remoteData = remoteScriptable;

            if (_remoteData != null)
                _url = GoogleUrlValidator.ValidateUrl(_remoteData.Url);
        }

        protected void FinishLoading()
        {
            LogResult();
            _request.Dispose();
        }

        protected void TryCreateDirectory()
        {
            var directoryPath = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
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
            {
                if (_settings.SaveAssetsAfterLoad)
                    _resultLog = string.IsNullOrEmpty(_dataHash) ? "downloaded" : "updated";
                else
                    _resultLog = "downloaded";
            }

            resultLogBuilder.AppendLine($"Data for [{_index}] {_name} {_resultLog}");

            if(_settings.SaveAssetsAfterLoad)
                resultLogBuilder.AppendLine($"File Path: {_filePath}");

#if UNITY_EDITOR
            var dataBytesCount = _request.downloadHandler.data == null ? 0 : _request.downloadHandler.data.Length;
            var size = FileExtensions.GetSizeString((ulong)dataBytesCount);
            resultLogBuilder.AppendLine($"Download Size: {size}");
#endif

            Logger.Log(resultLogBuilder.ToString());
        }

        protected async Task SaveResult()
        {
            var newHash = FileExtensions.GetHash(_request.downloadHandler.data);
            _result = new(_request.downloadHandler.data, newHash);

            if (_settings.SaveAssetsAfterLoad)
            {
                if (_remoteData.Hash == newHash) return;
                
                _filePath = _remoteData.GetFilePath();
                TryCreateDirectory();

                if (_request.downloadHandler.data != null)
                    await File.WriteAllBytesAsync(_filePath, _request.downloadHandler.data, cancellationToken: _token);
                else
                    await File.WriteAllTextAsync(_filePath, string.Empty, cancellationToken: _token);

#if UNITY_EDITOR
                UnityEditor.AssetDatabase.ImportAsset(_remoteData.GetAssetPath());
#endif
            }
        }
    }
}
