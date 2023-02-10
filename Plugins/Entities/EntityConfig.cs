using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    [CreateAssetMenu(fileName = "EntityConfig", menuName = "OpenFish/Entity")]
    [Serializable]
    public class EntityConfig : ScriptableObject
    {
        public string EntityId;
        public string EntityType;
        public List<string> RequiredSystems;


        public void Store(Dictionary<string, EntityConfig> IdConfigs, Dictionary<string, EntityConfig> TypeConfigs)
        {
            if (!String.IsNullOrEmpty(EntityId))
                IdConfigs[EntityId] = this;
            if (!String.IsNullOrEmpty(EntityType))
                TypeConfigs[EntityType] = this;
        }
    }
}