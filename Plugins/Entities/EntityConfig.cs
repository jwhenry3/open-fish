using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    [CreateAssetMenu(fileName = "EntityConfig", menuName = "OpenFish/Entity")]
    [Serializable]
    public class EntityConfig : ScriptableObject
    {
        public static Dictionary<string, EntityConfig> IdConfigs;
        public static Dictionary<string, EntityConfig> TypeConfigs;
        public string EntityId;
        public string EntityType;
        public List<string> RequiredSystems;
        
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