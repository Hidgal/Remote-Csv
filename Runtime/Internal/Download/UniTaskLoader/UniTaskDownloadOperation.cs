#if UNITASK_INSTALLED
using System.Threading;
using Cysharp.Threading.Tasks;
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

            await UpdateFile().AsUniTask();

            FinishLoading();
        }
    }
}

#endif