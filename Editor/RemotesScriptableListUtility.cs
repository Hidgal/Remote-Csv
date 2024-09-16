using RemoteCsv.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Logger = RemoteCsv.Internal.Logger;

namespace RemoteCsv.Editor
{
    public static class RemotesScriptableListUtility
    {
        [MenuItem("CONTEXT/ScriptableObject/Add To Remotes List")]
        public static void AddToRemotesList(MenuCommand command)
        {
            var type = command.context.GetType();
            if (RemoteTypesUtility.IsAvailableType(type))
            {
                TryCreateListAsset();

                if(RemoteScriptablesList.Instance.TryAddData(command.context as ScriptableObject))
                {
                    SelectRemotesList();
                }

                AssetDatabase.SaveAssetIfDirty(RemoteScriptablesList.Instance);
                AssetDatabase.Refresh();
            }
            else
            {
                Logger.LogError($"No data to parse from CSV in {command.context.name} (type: {type.Name})");
            }
        }

        [MenuItem("Tools/Remote Csv/Show Remotes List")]
        public static void SelectRemotesList()
        {
            TryCreateListAsset(true);
            Selection.activeObject = RemoteScriptablesList.Instance;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        public static void TryCreateListAsset(bool saveAssets = false)
        {
            if (!RemoteScriptablesList.Instance)
            {
                var listInstance = ScriptableObject.CreateInstance<RemoteScriptablesList>();
                listInstance.CreateDataArray();

                var path = Path.Combine("Assets", "Resources", $"{RemoteScriptablesList.DEFAULT_LIST_NAME}.asset");
                path = AssetDatabase.GenerateUniqueAssetPath(path);

                AssetDatabase.CreateAsset(listInstance, path);

                if (saveAssets)
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }

        public static List<ScriptableObject> FindAllAvailableAssets()
        {
            TryCreateListAsset();

            var availableTypes = RemoteTypesUtility.GetAvailableTypes();
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
                RemoteScriptablesList.Instance.TryAddData(asset);
            }

            RemoteScriptablesList.Instance.RemoveNullRefs();
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

