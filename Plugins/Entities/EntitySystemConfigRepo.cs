using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class EntitySystemConfigRepo<T> : ScriptableObject
    where T : EntitySystemConfig
    {
        public static Dictionary<string, T> IdConfigs;
        public static Dictionary<string, T> TypeConfigs;

        private void OnEnable()
        {
            IdConfigs = new Dictionary<string, T>();
            TypeConfigs = new Dictionary<string, T>();
            var configName = typeof(T).Name;
            foreach (var guid in AssetDatabase.FindAssets("t:" + configName))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
                if (asset == null) continue;
                if (!String.IsNullOrEmpty(asset.EntityId))
                    IdConfigs[asset.EntityId] = asset;
                if (!String.IsNullOrEmpty(asset.EntityType))
                    TypeConfigs[asset.EntityType] = asset;
            }
            Debug.Log("Found " + TypeConfigs.Count + " TypeConfigs for " + configName);
            Debug.Log("Found " + IdConfigs.Count + " IdConfigs for " + configName);
        }
        
    }
}