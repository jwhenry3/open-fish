using System;
using System.Collections.Generic;
using FishNet.Object;
using OpenFish.Core;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class EntitySystemManager<T> : NetworkBehaviour where T : EntitySystem
    {
        private Plugin plugin;
        protected T System;
        protected readonly Dictionary<string, GameObject> EntitySystems = new();

        protected virtual void Awake()
        {
            plugin = GetComponent<Plugin>();
            if (!plugin.enabled)
                plugin.Enabled += OnStarted;
            else
                OnStarted();
        }

        private void OnStarted()
        {
            foreach (var kvp in EntityManager.Entities)
                AddSystem(kvp.Value, true);
            EntityManager.EntityAdded += AddSystem;
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
        }

        private void OnDestroy()
        {
            if (plugin != null)
                plugin.Enabled -= OnStarted;
            if (plugin != null && !plugin.enabled) return;
            if (IsServer)
                EntityManager.EntityAdded -= AddSystem;
        }

        protected virtual void AddSystem(Entity entity, bool asServer)
        {
            if (!plugin.enabled) return;
            if (!IsServer || !asServer) return;
            if (entity.GetComponent<T>() != null)
                System = entity.AddExistingSystem<T>(entity.gameObject);
            if (System != null)
                EntitySystems[entity.EntityId] = System.gameObject;
        }

        public virtual List<NetworkObject> GetMovableNetworkObjects()
        {
            if (!plugin.enabled) return new();
            var list = new List<NetworkObject>();
            foreach (var nob in System.GetMovableNetworkObjects())
                list.Add(nob);
            return list;
        }
    }
}