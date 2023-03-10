using System;
using System.Collections.Generic;
using FishNet.Object;
using TriInspector;
using UnityEngine;

namespace OpenFish.Plugins.Entities
{
    [DeclareBoxGroup("manual", Title = "Configurable")]
    [DeclareBoxGroup("derived", Title = "Derived")]
    public class EntitySystem : NetworkBehaviour
    {
        [Group("manual")] public Entity Entity;

        protected virtual void Update()
        {
            
        }

        public virtual string GetSystemName()
        {
            return "undefined";
        }

        public virtual void OnEntityReady(bool asServer)
        {
            enabled = true;
            if (Entity.gameObject != gameObject && asServer)
            {
                transform.position = Vector3.up * 10000;
            } 
        }

        protected virtual void OnDestroy()
        {
            if (Entity != null)
                Entity.OnReady -= OnEntityReady;
        }

        public virtual List<NetworkObject> GetMovableNetworkObjects()
        {
            List<NetworkObject> list = new() { GetComponent<NetworkObject>() };
            return list;
        }

        protected override void Reset()
        {
            base.Reset();
            if (!Entity)
                Entity = GetComponent<Entity>();
        }
    }
}