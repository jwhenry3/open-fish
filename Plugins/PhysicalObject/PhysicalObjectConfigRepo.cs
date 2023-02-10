using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    [CreateAssetMenu(fileName = "PhysicalObjectConfigRepo", menuName = "OpenFish/PhysicalObject/Repo")]
    [Serializable]
    public class PhysicalObjectConfigRepo : ScriptableObject
    {
        public static Dictionary<string, PhysicalObjectConfig> IdConfigs;
        public static Dictionary<string, PhysicalObjectConfig> TypeConfigs;

        private void OnEnable()
        {
#if UNITY_EDITOR
            IdConfigs = new Dictionary<string, PhysicalObjectConfig>();
            TypeConfigs = new Dictionary<string, PhysicalObjectConfig>();

            foreach (var guid in AssetDatabase.FindAssets("t:PhysicalObjectConfig"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(PhysicalObjectConfig)) as PhysicalObjectConfig;
                if (asset == null) continue;
                asset.Store(IdConfigs, TypeConfigs);
            }
#endif
        }
    }
}