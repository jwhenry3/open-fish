using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    public class PhysicalObject : NetworkBehaviour
    {
        public static readonly Dictionary<string, PhysicalObject> Objects = new();
        [SyncVar]
        public string EntityId;
        public Transform ObjectHolder;
        public Transform CameraHolder;
        public Camera Camera;
        public Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Objects[EntityId] = this;
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(EntityId) && Objects.ContainsKey(EntityId))
                Objects.Remove(EntityId);
        }
    }
}