using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    public class EntityManager : NetworkBehaviour
    {
        public event Action<Entity, bool> EntityAdded;

        public EntityConfigRepo EntityConfigRepo;
        
        [SerializeField] private List<Entity> EntityList;
        private Dictionary<string, Entity> Entities;
        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Entities = Entities ?? new();
            base.NetworkManager.RegisterInstance(this);
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