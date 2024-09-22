using System.Collections;
using RemoteCsv.Settings;
using UnityEngine;

namespace RemoteCsv.Internal.Download.CoroutineLoader
{
    public class CoroutineDownloadService : AbstractDownloadService
    {
        private readonly MonoBehaviour _coroutineRunner;

        private Coroutine _downloadRoutine;

        public CoroutineDownloadService(RemoteCsvSettings settings, MonoBehaviour coroutineRunner, params IRemoteCsvData[] remotes)
            : base(settings, remotes)
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
            var coroutineArray = new IEnumerator[_remotes.Length];
            for (int i = 0; i < _remotes.Length; i++)
            {
                if (_remotes[i] == null) continue;

                var operation = new CoroutineDownloadOperation(_settings, i, _remotes[i], _cts.Token);
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
