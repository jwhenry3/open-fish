using System;
using System.Collections.Generic;
using OpenFish.Plugins.PhysicalObject;
using UnityEngine;

namespace OpenFish.Plugins.AI
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "OpenFish/AI")]
    [Serializable]
    public class AIConfig : ScriptableObject
    {
        public string EntityId;
        public string EntityType;
        public bool Wanders = false;
        public float WanderRadius = 10;
        

        public void Store(Dictionary<string, AIConfig> IdConfigs, Dictionary<string, AIConfig> TypeConfigs)
        {
            if (!String.IsNullOrEmpty(EntityId))
                IdConfigs[EntityId] = this;
            if (!String.IsNullOrEmpty(EntityType))
                TypeConfigs[EntityType] = this;
        }
    }
}