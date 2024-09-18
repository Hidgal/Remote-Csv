using System.Collections;
using UnityEngine;

namespace RemoteCsv.Internal.Download.CoroutineLoader
{
    public class CoroutineDownloadService : AbstractDownloadService
    {
        private readonly MonoBehaviour _coroutineRunner;

        private Coroutine _downloadRoutine;

        public CoroutineDownloadService(MonoBehaviour coroutineRunner, params IRemoteCsvData[] remotes)
            : base(default, remotes)
        {
            _coroutineRunner = coroutineRunner;
        }

        protected override void StartLoadingInternal()
        {
            if (!_coroutineRunner)
            {
                Logger.LogError("Can`t start loading, coroutine runner is null!");
                return;
            }

            _downloadRoutine = _coroutineRunner.StartCoroutine(LoadData());
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_downloadRoutine != null)
            {
                if (_coroutineRunner)
                    _coroutineRunner.StopCoroutine(_downloadRoutine);

                _downloadRoutine = null;
            }
        }

        private IEnumerator LoadData()
        {
            var loadOperations = new IEnumerator[_remotes.Count];
            for (int i = 0; i < _remotes.Count; i++)
            {
                if (_remotes[i] == null) continue;

                var operation = new CoroutineDownloadOperation(i, _remotes[i], _cts.Token, _forceRefreshLocalData);
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
