using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenFish.Plugins.Character
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "OpenFish/Characters")]
    [Serializable]
    public class CharacterConfig : ScriptableObject
    {
        public static Dictionary<string, CharacterConfig> IdConfigs;
        public static Dictionary<string, CharacterConfig> TypeConfigs;
        public string EntityId;
        public string EntityType;
        public string Name;

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