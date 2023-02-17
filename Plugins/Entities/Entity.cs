using System;
using System.Linq;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class Entity : NetworkBehaviour
    {
        public event Action<bool> OnReady;

        [SyncVar(OnChange = nameof(OnReadyChange))]
        public bool Ready;

        [SyncVar] public string EntityId;
        public string EntityType;

        public Vector3 OriginalPosition;

        public List<string> RequiredSystems => GetRequiredSystems();
        private List<string> _requiredSystems;
        public readonly List<string> LoadedSystems = new();
        public readonly Dictionary<string, EntitySystem> Systems = new();

        private List<string> GetRequiredSystems()
        {
            if (_requiredSystems != null) return _requiredSystems;
            List<string> systems = new();
            if (EntityConfigRepo.TypeConfigs.ContainsKey(EntityType))
            {
                foreach (var system in EntityConfigRepo.TypeConfigs[EntityType].RequiredSystems)
                    if (!systems.Contains(system))
                        systems.Add(system);
            }

            if (EntityConfigRepo.IdConfigs.ContainsKey(EntityId))
            {
                foreach (var system in EntityConfigRepo.IdConfigs[EntityId].RequiredSystems)
                    if (!systems.Contains(system))
                        systems.Add(system);
            }

            _requiredSystems = systems;
            return systems;
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            var t = transform;
            OriginalPosition = t.position;
            // move the entity out of the way so it does not impact observers
            t.position = Vector3.up * 10000;
            // when the network starts, entities that are already in the scene
            // may end up executing this before the manager is registered
            // with the network manager
            EntityManager.AddEntity(this, IsServer);
        }

        void OnReadyChange(bool previous, bool next, bool asServer)
        {
            if (!previous && next)
            {
                OnReady?.Invoke(asServer);
            }
        }


        public void OnDestroy()
        {
            EntityManager.RemoveEntity(this);
        }

        public T GetSystem<T>() where T : EntitySystem
        {
            var system = typeof(T).AssemblyQualifiedName;
            if (String.IsNullOrEmpty(system)) return null;
            if (Systems.ContainsKey(system))
                return (T)Systems[system];

            var component = GetComponentInChildren<T>();
            if (component == null) return null;
            Systems[system] = component;
            return component;
        }
        
        public T AddSystem<T>(NetworkObject prefab)
            where T : EntitySystem
        {
            if (!IsServer) return null;
            if (prefab == null) return null;
            var system = typeof(T).AssemblyQualifiedName;
            if (system == null) return null;
            var nob = NetworkManager.GetPooledInstantiated(prefab, true);
            var component = nob.GetComponent<T>();
            if (component == null) return null;
            if (!RequiredSystems.Contains(component.GetSystemName()))
            {
                // cancel the adding of the system, just destroy the instance
                Destroy(nob.gameObject);
                return null;
            }
            var t = nob.transform;
            t.localPosition = Vector3.zero;
            OnReady += component.OnEntityReady;
            if (component.GetSystemName() == "undefined")
                throw new Exception("Undefined System Name for " + component.name);
            component.Entity = this;
            component.enabled = false;
            component.gameObject.name = EntityId + ":" + component.GetSystemName();
            Systems[system] = component;
            LoadedSystems.Add(component.GetSystemName().ToLower());
            var count = RequiredSystems.Sum(systemName => LoadedSystems.Contains(systemName.ToLower()) ? 1 : 0);
            Spawn(nob, Owner);
            if (count >= RequiredSystems.Count)
                Ready = true;
            return component;
        }

        public T AddExistingSystem<T>(GameObject existingObject)
            where T : EntitySystem
        {
            var system = typeof(T).AssemblyQualifiedName;
            if (system == null) return null;
            var component = existingObject.GetComponent<T>();
            component.Entity = this;
            component.enabled = false;
            if (component.GetSystemName() == "undefined")
                throw new Exception("Undefined System Name for " + component.name);
            if (component.gameObject != gameObject)
                component.gameObject.name = EntityId + ":" + component.GetSystemName();
            Systems[system] = component;
            LoadedSystems.Add(component.GetSystemName().ToLower());
            OnReady += component.OnEntityReady;
            var count = RequiredSystems.Sum(systemName => LoadedSystems.Contains(systemName.ToLower()) ? 1 : 0);
            if (count >= RequiredSystems.Count)
                Ready = true;
            return component;
        }
        
        /**
         * Use this method when moving an entity between scenes in FishNet
         * so you can make sure all systems along with the entity are sent to the new scene
         */
        public NetworkObject[] GetMovableNetworkObjects()
        {
            var list = Systems.SelectMany(kvp => kvp.Value.GetMovableNetworkObjects()).ToList();
            list.Insert(0, GetComponent<NetworkObject>());
            return list.ToArray();
        }
    }
}