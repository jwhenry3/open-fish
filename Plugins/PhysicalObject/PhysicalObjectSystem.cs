using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
using TriInspector;
using UnityEngine;

namespace OpenFish.Plugins.PhysicalObject
{
    public class PhysicalObjectSystem : EntitySystem
    {
        public override string GetSystemName() => "physical-object";

        public event Action ObjectInstantiated;
        [Group("manual")] [SyncVar] public string Name;
        [Group("manual")] public NetworkObject Prefab;
        [Group("manual")] public PhysicalObjectConfig TypeConfig;
        [Group("manual")] public PhysicalObjectConfig IdConfig;

        [HideInInspector] public Transform Object;
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
            bool useSpawnPosition = false;
            Vector3 position = Vector3.zero;
            if (TypeConfig == null && PhysicalObjectConfigRepo.TypeConfigs.ContainsKey(Entity.EntityType))
                TypeConfig = PhysicalObjectConfigRepo.TypeConfigs[Entity.EntityType];

            if (IdConfig == null && PhysicalObjectConfigRepo.IdConfigs.ContainsKey(Entity.EntityId))
                IdConfig = PhysicalObjectConfigRepo.IdConfigs[Entity.EntityId];

            if (TypeConfig != null)
            {
                Name = TypeConfig.Name;
                prefab = TypeConfig.Prefab;
                if (TypeConfig.UseSpawnPosition)
                {
                    useSpawnPosition = true;
                    position = TypeConfig.SpawnPosition;
                }
            }

            if (IdConfig != null)
            {
                Name = IdConfig.Name;

                if (IdConfig.Prefab != null)
                    prefab = IdConfig.Prefab;
                if (IdConfig.UseSpawnPosition)
                {
                    useSpawnPosition = true;
                    position = IdConfig.SpawnPosition;
                }
            }

            if (Prefab != null)
                prefab = Prefab;
            // no prefab to spawn
            if (prefab == null) return;
            var instance = NetworkManager.GetPooledInstantiated(prefab, true);
            Object = instance.transform;
            Object.parent = t.parent;
            Object.position = useSpawnPosition ? position : Entity.OriginalPosition;
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