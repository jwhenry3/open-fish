using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class EntitySystemManager<T> : NetworkBehaviour where T : EntitySystem
    {
        public NetworkObject Prefab;
        protected T System;
        protected readonly Dictionary<string, GameObject> EntitySystems = new();

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            var manager = NetworkManager.GetInstance<EntityManager>();
            foreach (var kvp in manager.Entities)
                AddSystem(kvp.Value, true);
            manager.EntityAdded += AddSystem;
        }

        private void OnDestroy()
        {
            if (IsServer)
                NetworkManager.GetInstance<EntityManager>().EntityAdded -= AddSystem;
        }

        protected virtual void AddSystem(Entity entity, bool asServer)
        {
            if (!IsServer || !asServer) return;
            if (entity.GetComponent<T>() != null)
                System = entity.AddExistingSystem<T>(entity.gameObject);
            else
                System = entity.AddSystem<T>(Prefab);
            if (System != null)
                EntitySystems[entity.EntityId] = System.gameObject;
        }

        public virtual List<NetworkObject> GetMovableNetworkObjects()
        {
            var list = new List<NetworkObject>();
            foreach (var nob in System.GetMovableNetworkObjects())
                list.Add(nob);
            return list;
        }
    }
}