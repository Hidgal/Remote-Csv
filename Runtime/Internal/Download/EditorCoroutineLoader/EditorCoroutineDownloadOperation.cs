#if UNITY_EDITOR
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download.EditorCoroutineLoader
{
    public class EditorCoroutineDownloadOperation : AbstractDownloadOperation
    {
        public EditorCoroutineDownloadOperation(int index, IRemoteCsvData remoteScriptable, CancellationToken token, bool forceParseData)
            : base(index, remoteScriptable, token, forceParseData) { }

        public IEnumerator LoadData()
        {
            if (!IsInputDataValid()) yield break;

            _request = UnityWebRequest.Get(_url);

            yield return _request.SendWebRequest().WithCancellation(_token);

            if (!IsRequestValid()) yield break;

            yield return UpdateFile();

            FinishLoading();
        }
    }
}

#endif