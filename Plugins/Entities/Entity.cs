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
        
        private void AddEntityOnLoad(EntityManager manager)
        {
            Debug.Log("Add entity!");
            manager.AddEntity(this);
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            transform.position = Vector3.up * 5000;
            EntityManager.Loaded += AddEntityOnLoad;
            var instance = NetworkManager.GetInstance<EntityManager>();
            if (instance != null)
                instance.AddEntity(this);
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
            if (NetworkManager.GetInstance<EntityManager>())
                NetworkManager.GetInstance<EntityManager>().RemoveEntity(this);
            
            EntityManager.Loaded -= AddEntityOnLoad;
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
            Debug.Log("Adding system!");
            if (!IsServer) return null;
            if (prefab == null) return null;
            var system = typeof(T).AssemblyQualifiedName;
            if (system == null) return null;
            var nob = NetworkManager.GetPooledInstantiated(prefab, true);
            var component = nob.GetComponent<T>();
            Debug.Log(system);
            if (!RequiredSystems.Contains(component.GetSystemName()))
            {
                // cancel the adding of the system, just destroy the instance
                Destroy(nob.gameObject);
                return null;
            }
            var t = nob.transform;
            t.localPosition = Vector3.zero;
            component.Entity = this;
            component.enabled = false;
            component.gameObject.name = EntityId + ":" + component.GetSystemName();
            Systems[system] = component;
            Spawn(nob, Owner);
            LoadedSystems.Add(component.GetSystemName().ToLower());
            OnReady += component.OnEntityReady;
            var count = RequiredSystems.Sum(systemName => LoadedSystems.Contains(systemName.ToLower()) ? 1 : 0);
            Debug.Log(count);
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