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
        public event Action OnReady;

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

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            transform.position = Vector3.up * 5000;
            NetworkManager.GetInstance<EntityManager>().AddEntity(this);
        }

        void OnReadyChange(bool previous, bool next, bool asServer)
        {
            if (!previous && next && asServer)
            {
                OnReady?.Invoke();
            }
        }


        public void OnDestroy()
        {
            if (NetworkManager.GetInstance<EntityManager>())
                NetworkManager.GetInstance<EntityManager>().RemoveEntity(this);
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


        public T AddSystem<T>(NetworkObject prefab, bool parentToEntity)
            where T : EntitySystem
        {
            if (!IsServer) return null;
            if (prefab == null) return null;
            var system = typeof(T).AssemblyQualifiedName;
            if (system == null) return null;
            var nob = NetworkManager.GetPooledInstantiated(prefab, true);
            var component = nob.GetComponent<T>();

            if (!RequiredSystems.Contains(component.GetSystemName()))
            {
                // cancel the adding of the system, just destroy the instance
                Destroy(nob.gameObject);
                return null;
            }
            var t = nob.transform;
            if (parentToEntity)
                t.parent = transform;
            t.localPosition = Vector3.zero;
            component.Entity = this;
            component.enabled = false;
            Systems[system] = component;
            Spawn(nob, Owner);
            LoadedSystems.Add(component.GetSystemName().ToLower());
            OnReady += component.OnEntityReady;
            var count = 0;
            foreach (var systemName in RequiredSystems)
                count += LoadedSystems.Contains(systemName.ToLower()) ? 1 : 0;
            if (count >= RequiredSystems.Count)
                Ready = true;
            return component;
        }
    }
}