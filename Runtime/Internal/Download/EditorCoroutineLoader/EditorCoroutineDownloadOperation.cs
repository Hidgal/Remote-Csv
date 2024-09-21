#if UNITY_EDITOR
using System.Collections;
using System.Threading;
using RemoteCsv.Settings;
using UnityEngine.Networking;

namespace RemoteCsv.Internal.Download.EditorCoroutineLoader
{
    public class EditorCoroutineDownloadOperation : AbstractDownloadOperation
    {
        public EditorCoroutineDownloadOperation(RemoteCsvSettings settings, int index, IRemoteCsvData remoteScriptable, CancellationToken token)
            : base(settings, index, remoteScriptable, token) { }

        public IEnumerator LoadData()
        {
            if (!IsInputDataValid()) yield break;

            _request = UnityWebRequest.Get(_url);

            yield return _request.SendWebRequest();

            if (!IsRequestValid()) yield break;

            yield return SaveResult();

            FinishLoading();
        }
    }
}

#endif