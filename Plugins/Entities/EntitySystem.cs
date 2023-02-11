using System;
using System.Collections.Generic;
using FishNet.Object;

namespace OpenFish.Plugins.Entities
{
    public class EntitySystem : NetworkBehaviour
    {
        
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
    }
}