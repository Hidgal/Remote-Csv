#if UNITASK_INSTALLED
using System.Threading;
using Cysharp.Threading.Tasks;
using RemoteCsv.Settings;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download.UniTaskLoader
{
    public class UniTaskDownloadOperation : AbstractDownloadOperation
    {
        public UniTaskDownloadOperation(RemoteCsvSettings settings, int index, IRemoteCsvData remoteScriptable, CancellationToken token)
            : base(settings, index, remoteScriptable, token) { }

        public async UniTask LoadData()
        {
            if (!IsInputDataValid()) return;

            _request = UnityWebRequest.Get(_url);

            await _request.SendWebRequest().WithCancellation(_token);

            if (!IsRequestValid()) return;

            await SaveResult().AsUniTask();

            FinishLoading();
        }
    }
}

#endif