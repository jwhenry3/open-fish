using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    public class PhysicalObjectSystem : EntitySystem
    {
        public override string GetSystemName() => "physical-object";

        [SyncVar] public string Name;
        public PhysicalObjectConfig TypeConfig;
        public PhysicalObjectConfig IdConfig;

        public Transform Object;
        private Transform t;

        private void Awake()
        {
            t = transform;
        }

        public override void OnEntityReady()
        {
            base.OnEntityReady();
            if (!IsServer) return;
            NetworkObject prefab = null;
            if (PhysicalObjectConfig.TypeConfigs.ContainsKey(Entity.EntityType))
            {
                TypeConfig = PhysicalObjectConfig.TypeConfigs[Entity.EntityType];
                Name = TypeConfig.Name;
                prefab = TypeConfig.Prefab;
            }

            if (PhysicalObjectConfig.IdConfigs.ContainsKey(Entity.EntityId))
            {
                IdConfig = PhysicalObjectConfig.IdConfigs[Entity.EntityId];
                Name = IdConfig.Name;
                prefab = IdConfig.Prefab != null ? IdConfig.Prefab : TypeConfig ? TypeConfig.Prefab : null;
            }

            if (prefab == null) return;
            var instance = NetworkManager.GetPooledInstantiated(prefab, true);
            Object = instance.transform;
            Spawn(instance, Owner);
        }
        
        private float tick;
        private void Update()
        {
            if (t != null && Object != null)
            {
                tick += Time.deltaTime;
                if (!(tick > 1)) return;
                t.position = Object.position;
                tick = 0;
            }
        }
    }
}