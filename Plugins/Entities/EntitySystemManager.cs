using System.Linq;
using FishNet.Object;

namespace OpenFish.Plugins.Entities
{
    public class EntitySystemManager<T> : NetworkBehaviour where T : EntitySystem
    {
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

        private void AddSystem(Entity entity, bool asServer)
        {
            if (!IsServer || !asServer) return;
            entity.AddSystem<T>(Prefab);
        }
    }
}