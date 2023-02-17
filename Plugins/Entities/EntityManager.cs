using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class EntityManager : NetworkBehaviour
    {
        public static event Action<EntityManager> Loaded;
        public static event Action<Entity, bool> EntityAdded;

        public EntityConfigRepo EntityConfigRepo;
        
        private static List<Entity> EntityList = new();
        public static Dictionary<string, Entity> Entities = new();
        public static EntityManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            EntityList = new();
            Entities = new();
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
            Loaded?.Invoke(this);
        }

        public static void AddEntity(Entity entity, bool asServer)
        {
            if (!EntityList.Contains(entity)) EntityList.Add(entity);
            Entities[entity.EntityId] = entity;
            EntityAdded?.Invoke(entity, asServer);
        }

        public static void RemoveEntity(Entity entity)
        {
            if (EntityList.Contains(entity)) EntityList.Remove(entity);
            if (Entities.ContainsKey(entity.EntityId)) Entities.Remove(entity.EntityId);
        }

        public static Entity GetEntity(string entityId)
        {
            if (!Entities.ContainsKey(entityId)) return null;
            return Entities[entityId];
        } 
        
    }
}