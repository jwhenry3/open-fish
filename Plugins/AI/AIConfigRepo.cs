using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenFish.Plugins.AI
{
    [CreateAssetMenu(fileName = "AIConfigRepo", menuName = "OpenFish/AI/Repo")]
    [Serializable]
    public class AIConfigRepo : ScriptableObject
    {
        public static Dictionary<string, AIConfig> IdConfigs;
        public static Dictionary<string, AIConfig> TypeConfigs;

        private void OnEnable()
        {
#if UNITY_EDITOR
            IdConfigs = new Dictionary<string, AIConfig>();
            TypeConfigs = new Dictionary<string, AIConfig>();

            foreach (var guid in AssetDatabase.FindAssets("t:PhysicalObjectConfig"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(AIConfig)) as AIConfig;
                if (asset == null) continue;
                asset.Store(IdConfigs, TypeConfigs);
            }
#endif
        }
    }
}