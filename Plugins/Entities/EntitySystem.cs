using System;
using FishNet.Object;

namespace OpenFish.Plugins.Entities
{
    public class EntitySystem : NetworkBehaviour
    {
        public string SystemName;
        public Entity Entity;

        public virtual void OnEntityReady()
        {
            
        }

        protected virtual void OnDestroy()
        {
            Entity.OnReady -= OnEntityReady;
        }
    }
}