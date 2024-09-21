using System.Linq;
using RemoteCsv.Internal;
using RemoteCsv.Settings;
using UnityEngine;

namespace RemoteCsv.Editor
{
    public static class SettingsAssetExtension
    {
        public static bool TryAddData(this RemoteCsvSettingsAsset settingsAsset, ScriptableObject scriptable)
        {
            if (!settingsAsset.HasData(scriptable))
            {
                var tempArray = new RemoteCsvData[settingsAsset.InternalDataArray.Length + 1];
                settingsAsset.InternalDataArray.CopyTo(tempArray, 0);
                tempArray[^1] = new(scriptable);
                settingsAsset.InternalDataArray = tempArray;

                UnityEditor.EditorUtility.SetDirty(settingsAsset);
                return true;
            }

            return false;
        }

        public static void RemoveNullRefs(this RemoteCsvSettingsAsset settingsAsset)
        {
            settingsAsset.InternalDataArray = settingsAsset.InternalDataArray.Where(data => data.TargetScriptable).ToArray();
            UnityEditor.EditorUtility.SetDirty(settingsAsset);
        }

        public static void CreateDataArray(this RemoteCsvSettingsAsset settingsAsset)
        {
            settingsAsset.InternalDataArray = new RemoteCsvData[0];
        }
    }
}
