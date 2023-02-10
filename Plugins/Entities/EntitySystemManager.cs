using System;
using System.Linq;
using FishNet.Object;

namespace OpenFish.Plugins.Entities
{
    public class EntitySystemManager<T> : NetworkBehaviour where T : EntitySystem
    {
        protected virtual bool ParentToEntity() => true;
        public NetworkObject Prefab;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkManager.GetInstance<EntityManager>().EntityAdded += AddSystem;
        }

        private void OnDestroy()
        {
            if (IsServer)
                NetworkManager.GetInstance<EntityManager>().EntityAdded -= AddSystem;
        }

        protected virtual void AddSystem(Entity entity, bool asServer)
        {
            if (!IsServer || !asServer) return;
            entity.AddSystem<T>(Prefab, ParentToEntity());
        }
    }
}