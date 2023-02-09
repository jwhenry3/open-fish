using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    [CreateAssetMenu(fileName = "PhysicalObjectConfig", menuName = "OpenFish/PhysicalObject")]
    [Serializable]
    public class PhysicalObjectConfig : ScriptableObject
    {
        public static Dictionary<string, PhysicalObjectConfig> IdConfigs;
        public static Dictionary<string, PhysicalObjectConfig> TypeConfigs;
        public string EntityId;
        public string EntityType;
        public string Name;
        public NetworkObject Prefab;

        private void OnEnable()
        {
            IdConfigs = IdConfigs ?? new();
            TypeConfigs = TypeConfigs ?? new();
            if (!String.IsNullOrEmpty(EntityId))
                IdConfigs[EntityId] = this;
            if (!String.IsNullOrEmpty(EntityType))
                TypeConfigs[EntityType] = this;
        }
    }
}