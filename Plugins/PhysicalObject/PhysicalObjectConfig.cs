using System;
using FishNet.Object;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    [CreateAssetMenu(fileName = "PhysicalObjectConfig", menuName = "OpenFish/PhysicalObject")]
    [Serializable]
    public class PhysicalObjectConfig : EntitySystemConfig
    {
        public string Name;
        public NetworkObject Prefab;
        public bool UseSpawnPosition;
        public Vector3 SpawnPosition;

    }
}