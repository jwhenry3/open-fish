using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Newtonsoft.Json;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class Entity : NetworkBehaviour
    {
        public event Action OnReady;
        [SyncVar(OnChange = nameof(OnReadyChange))] public bool Ready;
        [SyncVar] public string EntityId;
        public string EntityType;

        public string[] RequiredSystems;
        public readonly List<string> LoadedSystems = new();
        public readonly Dictionary<string, EntitySystem> Systems = new();

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
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

        public T AddSystem<T>(NetworkObject prefab) where T : EntitySystem
        {
            if (!IsServer) return null;
            var system = typeof(T).AssemblyQualifiedName;
            if (String.IsNullOrEmpty(system)) return null;
            var nob = NetworkManager.GetPooledInstantiated(prefab, true);
            var t = nob.transform;
            t.parent = transform;
            t.localPosition = Vector3.zero;
            var component = nob.GetComponent<T>();
            component.Entity = this;
            component.enabled = false;
            Systems[system] = component;
            Spawn(nob, Owner);
            LoadedSystems.Add(component.GetSystemName().ToLower());
            OnReady += component.OnEntityReady;
            var count = 0;
            foreach (var systemName in RequiredSystems)
                count += LoadedSystems.Contains(systemName.ToLower()) ? 1 : 0;

            if (count == RequiredSystems.Length)
                Ready = true;
            return component;
        }
    }
}