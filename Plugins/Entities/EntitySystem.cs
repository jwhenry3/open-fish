using System;
using System.Collections.Generic;
using FishNet.Object;
using TriInspector;

namespace OpenFish.Plugins.Entities
{
    [DeclareBoxGroup("manual", Title = "Can Set Manually")]
    public class EntitySystem : NetworkBehaviour
    {
        [Group("manual")]
        public Entity Entity;
        
        
        public virtual string GetSystemName()
        {
            return "undefined";
        }
        public virtual void OnEntityReady(bool asServer)
        {
            enabled = true;
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