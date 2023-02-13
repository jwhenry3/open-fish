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
        public string EntityId;
        public string EntityType;
        public string Name;
        public NetworkObject Prefab;
        public Vector3 SpawnPosition;

        public void Store(Dictionary<string,PhysicalObjectConfig> IdConfigs, Dictionary<string,PhysicalObjectConfig> TypeConfigs)
        {
            if (!String.IsNullOrEmpty(EntityId))
                IdConfigs[EntityId] = this;
            if (!String.IsNullOrEmpty(EntityType))
                TypeConfigs[EntityType] = this;
        }
    }
}