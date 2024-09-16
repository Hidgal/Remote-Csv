using System;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using RemoteCsv.Internal.Extensions;
using RemoteCsv.Internal.Parsers;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download
{
    public class AsyncDownloadOperation
    {
        private readonly CancellationToken _token;
        private readonly IRemoteCsvData _remoteData;
        private readonly bool _forceParseData;
        private readonly int _index;

        private string _filePath;
        private UnityWebRequest _request;
        private string _currentHash;
        private string _resultLog;

#if UNITY_EDITOR
        private DateTime _startDate;
#endif

        private string _url => _remoteData.Url;
        private string _name => _remoteData.FileName;
        private string _dataHash => _remoteData.Hash;
        private string _fileNameWithExtension => _remoteData.Extension;

        public AsyncDownloadOperation(int index, IRemoteCsvData remoteScriptable, CancellationToken token, bool forceParseData)
        {
            _index = index;
            _token = token;
            _remoteData = remoteScriptable;
            _forceParseData = forceParseData;
        }

        public async UniTask LoadData()
        {
            if (!IsInputDataValid()) return;

#if UNITY_EDITOR
            _startDate = DateTime.Now;
#endif

            var validUrl = GoogleUrlValidator.ValidateUrl(_url);
            _request = UnityWebRequest.Get(validUrl);

            await _request.SendWebRequest().WithCancellation(_token);

            if (!IsRequestValid()) return;

            _filePath = _remoteData.GetFilePath();
            _currentHash = GetCurrentHash();

            await UpdateFile();

            if(_remoteData.AutoParseAfterLoad || _forceParseData)
            {
                ObjectParser.ParseObject(_remoteData.TargetScriptable, _remoteData.GetFilePath());
            }

            _request.Dispose();
            _remoteData.OnDataLoaded();

            LogResult();
        }

        private async UniTask UpdateFile()
        {
            if (!string.IsNullOrEmpty(_currentHash))
            {
                var hashSum = FileExtensions.GetHash(_request.downloadHandler.data);

                //No need to import asset if hashes are equal
                if (_currentHash == hashSum)
                {
                    _resultLog = "is already up to date";
                    return;
                }
            }

            var directoryPath = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (_request.downloadHandler.data != null)
                await File.WriteAllBytesAsync(_filePath, _request.downloadHandler.data, cancellationToken: _token).AsUniTask();
            else
                await File.WriteAllTextAsync(_filePath, string.Empty, cancellationToken: _token).AsUniTask();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(_remoteData.GetAssetPath());
#endif
        }

        private string GetCurrentHash()
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

        private bool IsRequestValid()
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

        private bool IsInputDataValid()
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

        private void LogResult()
        {
            var resultLogBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(_resultLog))
                _resultLog = string.IsNullOrEmpty(_currentHash) ? "downloaded" : "updated";

            resultLogBuilder.AppendLine($"[{_index}] {_name}: {_resultLog}");
            resultLogBuilder.AppendLine($"File Path: {_filePath}");

#if UNITY_EDITOR
            var size = FileExtensions.GetSizeString((ulong)new FileInfo(_filePath).Length);
            resultLogBuilder.AppendLine($"Download Size: {size}");

            var downloadTime = DateTime.Now - _startDate;
            resultLogBuilder.AppendLine($"Load Time: {downloadTime:mm\\:ss\\:ff}");
#endif

            Logger.Log(resultLogBuilder.ToString());
        }
    }
}
