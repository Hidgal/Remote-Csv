using RemoteCsv.Internal.Extensions;
using Logger = RemoteCsv.Internal.Logger;
using RemoteCsv.Settings;
using System;
using System.IO;

namespace RemoteCsv
{
    public abstract class AbstractRemoteCsvService : IRemoteCsvService
    {
        public event Action OnFinished;

        protected readonly IRemoteCsvData[] _remotes;
        protected readonly RemoteCsvSettings _settings;

        protected bool _isFinished;
        protected IDownloadService _downloadService;

        public bool IsFinished => _isFinished;

        public AbstractRemoteCsvService(RemoteCsvSettings settings, IRemoteCsvData[] remotes)
        {
            _remotes = remotes;
            _settings = settings;
        }

        public virtual void Start()
        {
            if (_settings == null)
            {
                Logger.LogError("RemoteCsvSettings is null, can`t start loading!");
                CallFinish();
                return;
            }

            _downloadService = GetDownloadService();
            if (_downloadService == null)
            {
                Logger.LogError("Can`t get DownloadService! Try to use another method or install recomended dependencies.");
                CallFinish();
                return;
            }

            _downloadService.OnLoadFinish += OnDownloadFinished;
            StartLoading();
        }

        public virtual void Dispose()
        {
            CallFinish();

            if (_downloadService != null)
            {
                _downloadService.OnLoadFinish -= OnDownloadFinished;
                _downloadService.Dispose();
            }
        }

        protected abstract IDownloadService GetDownloadService();
        protected abstract void StartLoading();

        protected void OnDownloadFinished()
        {
            bool parseResult;
            for (int i = 0; i < _remotes.Length; i++)
            {
                if (_remotes[i] == null) continue;
                if (!_remotes[i].TargetScriptable) continue;

                if (_settings.SaveAssetsAfterLoad)
                {
                    string filePath = _remotes[i].GetFilePath();
                    if (_downloadService.IsSuccessed && string.IsNullOrEmpty(filePath) == false && File.Exists(filePath))
                    {
                        parseResult = RemoteCsvParser.ParseObject(_remotes[i].TargetScriptable, filePath);
                    }
                    else
                    {
                        parseResult = false;
                    }
                }
                else
                {
                    if (_downloadService.IsSuccessed)
                    {
                        parseResult = RemoteCsvParser.ParseObject(_remotes[i].TargetScriptable, _downloadService.Result[i].Data);
                    }
                    else
                    {
                        parseResult = false;
                    }
                }

                if (_settings.SaveAssetsAfterLoad && parseResult)
                {
                    _remotes[i].UpdateHash(_downloadService.Result[i].Hash);

#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(_remotes[i].TargetScriptable);
                    UnityEditor.EditorUtility.SetDirty(RemoteCsvSettingsAsset.Instance);
#endif
                }

            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif

            CallFinish();
        }

        protected void CallFinish()
        {
            Logger.Log("All processes finished.");
            _isFinished = true;
            OnFinished?.Invoke();
        }
    }
}
