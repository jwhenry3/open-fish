using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    [CreateAssetMenu(fileName = "EntityConfig", menuName = "OpenFish/Entity")]
    [Serializable]
    public class EntityConfig : EntitySystemConfig
    {
        public List<string> RequiredSystems;
    }
}