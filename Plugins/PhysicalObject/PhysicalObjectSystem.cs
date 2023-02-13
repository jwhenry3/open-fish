using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    public class PhysicalObjectSystem : EntitySystem
    {
        public override string GetSystemName() => "physical-object";

        public event Action ObjectInstantiated;
        [SyncVar] public string Name;
        public PhysicalObjectConfig TypeConfig;
        public PhysicalObjectConfig IdConfig;

        public Transform Object;
        private Transform t;

        private void Awake()
        {
            t = transform;
        }

        public override void OnEntityReady(bool asServer)
        {
            base.OnEntityReady(asServer);
            if (!asServer) return;
            NetworkObject prefab = null;
            Vector3 position = Vector3.zero;
            if (PhysicalObjectConfigRepo.TypeConfigs.ContainsKey(Entity.EntityType))
            {
                TypeConfig = PhysicalObjectConfigRepo.TypeConfigs[Entity.EntityType];
                Name = TypeConfig.Name;
                prefab = TypeConfig.Prefab;
                position = TypeConfig.SpawnPosition;
            }

            if (PhysicalObjectConfigRepo.IdConfigs.ContainsKey(Entity.EntityId))
            {
                IdConfig = PhysicalObjectConfigRepo.IdConfigs[Entity.EntityId];
                Name = IdConfig.Name;
                prefab = IdConfig.Prefab != null ? IdConfig.Prefab : TypeConfig ? TypeConfig.Prefab : null;
                position = IdConfig.SpawnPosition;
            }

            if (prefab == null) return;
            var instance = NetworkManager.GetPooledInstantiated(prefab, true);
            Object = instance.transform;
            Object.parent = t.parent;
            Object.position = position;
            instance.gameObject.name = Entity.EntityId + ":physical-object:instance";
            Spawn(instance, Owner);
            ObjectInstantiated?.Invoke();
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

        public override List<NetworkObject> GetMovableNetworkObjects()
        {
            var list = base.GetMovableNetworkObjects();
            list.Add(Object.GetComponent<NetworkObject>());
            return list;
        }
    }
}