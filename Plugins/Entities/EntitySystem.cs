using System;
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
        public virtual void OnEntityReady()
        {
            
        }

        protected virtual void OnDestroy()
        {
            if (Entity != null)
                Entity.OnReady -= OnEntityReady;
        }
    }
}