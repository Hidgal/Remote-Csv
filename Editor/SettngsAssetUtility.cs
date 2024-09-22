using System.Collections.Generic;
using RemoteCsv.Settings;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv.Editor
{
    public static class SettngsAssetUtility
    {
        [MenuItem("CONTEXT/ScriptableObject/Add To Remotes List")]
        public static void AddToRemotesList(MenuCommand command)
        {
            var type = command.context.GetType();
            if (RemoteCsvTypeUtility.IsAvailableType(type))
            {
                var wasChanged = TryCreateListAsset();
                wasChanged |= RemoteCsvSettingsAsset.Instance.TryAddData(command.context as ScriptableObject);

                if (wasChanged)
                {
                    AssetDatabase.SaveAssetIfDirty(RemoteCsvSettingsAsset.Instance);
                    AssetDatabase.Refresh();
                }

                SelectRemotesList();
            }
            else
            {
                Logger.LogError($"No data to parse from CSV in {command.context.name} (type: {type.Name})");
            }
        }

        [MenuItem("Tools/Remote Csv/Collect All Remotes")]
        public static void CollectAllRemotes()
        {
            FindAllAvailableAssets();
            Selection.activeObject = RemoteCsvSettingsAsset.Instance;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        [MenuItem("Tools/Remote Csv/Show Remotes List")]
        public static void SelectRemotesList()
        {
            TryCreateListAsset(true);
            Selection.activeObject = RemoteCsvSettingsAsset.Instance;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        public static bool TryCreateListAsset(bool saveAssets = false)
        {
            if (!RemoteCsvSettingsAsset.Instance)
            {
                var listInstance = ScriptableObject.CreateInstance<RemoteCsvSettingsAsset>();
                listInstance.CreateDataArray();

                var absoluteResourcesPath = Path.Combine(Application.dataPath, "Resources");
                if (!Directory.Exists(absoluteResourcesPath))
                {
                    Directory.CreateDirectory(absoluteResourcesPath);
                    AssetDatabase.Refresh();
                }

                var path = Path.Combine("Assets", "Resources", $"{RemoteCsvSettingsAsset.DEFAULT_LIST_NAME}.asset");
                path = AssetDatabase.GenerateUniqueAssetPath(path);

                AssetDatabase.CreateAsset(listInstance, path);

                if (saveAssets)
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return true;
            }

            return false;
        }

        public static List<ScriptableObject> FindAllAvailableAssets()
        {
            TryCreateListAsset();

            var availableTypes = RemoteCsvTypeUtility.GetAvailableTypes();
            List<ScriptableObject> availableAssets = new();
            foreach (var type in availableTypes)
            {
                availableAssets.AddRange(GetAssetsOfType(type));
            }

            CheckDataArray(availableAssets);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return availableAssets;
        }

        private static void CheckDataArray(List<ScriptableObject> availableAssets)
        {
            foreach (var asset in availableAssets)
            {
                RemoteCsvSettingsAsset.Instance.TryAddData(asset);
            }

            RemoteCsvSettingsAsset.Instance.RemoveNullRefs();
        }

        private static List<ScriptableObject> GetAssetsOfType(Type type)
        {
            var result = new List<ScriptableObject>();
            var assetPaths = GetAssetPaths($"t:{type.Name}");

            foreach (var assetPath in assetPaths)
            {
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                if (asset)
                {
                    result.Add(asset);
                }
            }

            return result;
        }
        private static string[] GetAssetPaths(string filter)
        {
            var guids = AssetDatabase.FindAssets(filter);

            return guids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
        }
    }
}

