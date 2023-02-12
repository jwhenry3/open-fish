using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class EntityManager : NetworkBehaviour
    {
        public static event Action<EntityManager> Loaded;
        public event Action<Entity, bool> EntityAdded;

        public EntityConfigRepo EntityConfigRepo;
        
        [SerializeField] private List<Entity> EntityList;
        public readonly Dictionary<string, Entity> Entities = new();


        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
            Loaded?.Invoke(this);
        }

        public void AddEntity(Entity entity)
        {
            if (!EntityList.Contains(entity)) EntityList.Add(entity);
            Entities[entity.EntityId] = entity;
            EntityAdded?.Invoke(entity, IsServer);
        }

        public void RemoveEntity(Entity entity)
        {
            if (EntityList.Contains(entity)) EntityList.Remove(entity);
            if (Entities.ContainsKey(entity.EntityId)) Entities.Remove(entity.EntityId);
        }

        public Entity GetEntity(string entityId)
        {
            if (!Entities.ContainsKey(entityId)) return null;
            return Entities[entityId];
        } 
        
    }
}