#if UNITY_EDITOR
using System.Collections;
using System.Threading;
using Unity.EditorCoroutines.Editor;

namespace RemoteCsv.Internal.Download.EditorCoroutineLoader
{
    public class EditorCoroutineDownloadService : AbstractDownloadService
    {
        private EditorCoroutine _coroutine;

        public EditorCoroutineDownloadService(CancellationToken token = default, params IRemoteCsvData[] remotes)
            : base(token, remotes) { }

        protected override void StartLoadingInternal()
        {
            _coroutine = EditorCoroutineUtility.StartCoroutine(LoadData(), this);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_coroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator LoadData()
        {
            var loadOperations = new IEnumerator[_remotes.Count];
            for (int i = 0; i < _remotes.Count; i++)
            {
                if (_remotes[i] == null) continue;

                var operation = new EditorCoroutineDownloadOperation(i, _remotes[i], _cts.Token, _forceRefreshLocalData);
                loadOperations[i] = operation.LoadData();
            }

            for (int i = 0; i < loadOperations.Length; i++)
            {
                yield return loadOperations[i];
            }

            OnLoadingFinished();
        }
    }
}

#endif