using RemoteCsv.Settings;

namespace RemoteCsv.Internal.Utility
{
    public static class RemoteCsvSettingsUtility
    {
        public static RemoteCsvSettings GetSettingsFromAsset()
        {
            if (!RemoteCsvSettingsAsset.Instance)
            {
                Logger.LogError("Can`t find RemoteCsv Settings Asset! Create it in Assets/Resources Folder");
                return null;
            }

            return RemoteCsvSettingsAsset.Instance.Settings;
        }
    }
}
