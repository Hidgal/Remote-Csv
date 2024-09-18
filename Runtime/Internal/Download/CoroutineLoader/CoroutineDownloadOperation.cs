using System.Collections;
using System.Threading;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download.CoroutineLoader
{
    public class CoroutineDownloadOperation : AbstractDownloadOperation
    {
        public CoroutineDownloadOperation(int index, IRemoteCsvData remoteScriptable, CancellationToken token, bool forceParseData)
            : base(index, remoteScriptable, token, forceParseData) { }
        
        public IEnumerator LoadData()
        {
            if (!IsInputDataValid()) yield break;

            _request = UnityWebRequest.Get(_url);

            yield return _request.SendWebRequest();

            if (!IsRequestValid()) yield break;

            yield return UpdateFile();

            FinishLoading();
        }
    }
}
