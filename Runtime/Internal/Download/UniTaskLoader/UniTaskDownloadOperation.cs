using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using RemoteCsv.Internal.Extensions;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download.UniTaskLoader
{
    public class UniTaskDownloadOperation : AbstractDownloadOperation
    {
        public UniTaskDownloadOperation(int index, IRemoteCsvData remoteScriptable, CancellationToken token, bool forceParseData)
            : base(index, remoteScriptable, token, forceParseData) { }

        public async UniTask LoadData()
        {
            if (!IsInputDataValid()) return;

            _request = UnityWebRequest.Get(_url);

            await _request.SendWebRequest().WithCancellation(_token);

            if (!IsRequestValid()) return;

            _filePath = _remoteData.GetFilePath();
            _currentHash = GetCurrentHash();

            await UpdateFile();

            FinishLoading();
        }

        protected async UniTask UpdateFile()
        {
            if (!IsHashChanged()) return;

            TryCreateDirectory();

            if (_request.downloadHandler.data != null)
                await File.WriteAllBytesAsync(_filePath, _request.downloadHandler.data, cancellationToken: _token).AsUniTask();
            else
                await File.WriteAllTextAsync(_filePath, string.Empty, cancellationToken: _token).AsUniTask();

            ImportCsvAsset();
        }
    }
}
