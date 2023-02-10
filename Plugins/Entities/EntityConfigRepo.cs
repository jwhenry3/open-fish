using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    [CreateAssetMenu(fileName = "EntityConfigRepo", menuName = "OpenFish/Entity/Repo")]
    [Serializable]
    public class EntityConfigRepo : ScriptableObject
    {
        public static Dictionary<string, EntityConfig> IdConfigs;
        public static Dictionary<string, EntityConfig> TypeConfigs;

        private void OnEnable()
        {
#if UNITY_EDITOR
            IdConfigs = new Dictionary<string, EntityConfig>();
            TypeConfigs = new Dictionary<string, EntityConfig>();

            foreach (var guid in AssetDatabase.FindAssets("t:EntityConfig"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(EntityConfig)) as EntityConfig;
                if (asset == null) continue;
                asset.Store(IdConfigs, TypeConfigs);
            }
#endif
        }
    }
}