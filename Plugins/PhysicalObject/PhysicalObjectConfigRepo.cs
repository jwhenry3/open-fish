using System;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    [CreateAssetMenu(fileName = "PhysicalObjectConfigRepo", menuName = "OpenFish/PhysicalObject/Repo")]
    [Serializable]
    public class PhysicalObjectConfigRepo : EntitySystemConfigRepo<PhysicalObjectConfig>
    {
    }
}