using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    public class PhysicalObject : NetworkBehaviour
    {
        [SyncVar]
        public string EntityId;
        public Transform ObjectHolder;
        public Transform CameraHolder;
        public Camera Camera;
    }
}