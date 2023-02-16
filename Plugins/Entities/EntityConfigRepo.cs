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
    public class EntityConfigRepo : EntitySystemConfigRepo<EntityConfig>
    {
    }
}