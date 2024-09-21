#if UNITY_EDITOR
using System.Collections;
using RemoteCsv.Settings;
using Unity.EditorCoroutines.Editor;

namespace RemoteCsv.Internal.Download.EditorCoroutineLoader
{
    public class EditorCoroutineDownloadService : AbstractDownloadService
    {
        private EditorCoroutine _downloadRoutine;

        public EditorCoroutineDownloadService(RemoteCsvSettings settings, params IRemoteCsvData[] remotes)
            : base(settings, remotes) { }

        protected override void StartLoadingInternal()
        {
            _downloadRoutine = EditorCoroutineUtility.StartCoroutine(LoadData(), this);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_downloadRoutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(_downloadRoutine);
                _downloadRoutine = null;
            }
        }

        private IEnumerator LoadData()
        {
            var coroutineArray = new IEnumerator[_remotes.Length];
            for (int i = 0; i < _remotes.Length; i++)
            {
                if (_remotes[i] == null) continue;

                var operation = new EditorCoroutineDownloadOperation(_settings, i, _remotes[i], _cts.Token);
                _operations[i] = operation;
                coroutineArray[i] = operation.LoadData();
            }

            for (int i = 0; i < coroutineArray.Length; i++)
            {
                yield return coroutineArray[i];
            }

            OnLoadingFinished();
        }
    }
}

#endif