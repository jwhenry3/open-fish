using System.Linq;
using FishNet.Object;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.PhysicalObject
{
    public class PhysicalObjectManager : NetworkBehaviour
    {
        public NetworkObject CharacterPrefab;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkManager.GetInstance<EntityManager>().EntityAdded += AddCharacter;
        }

        private void OnDestroy()
        {
            if (IsServer)
                NetworkManager.GetInstance<EntityManager>().EntityAdded -= AddCharacter;
        }

        void AddCharacter(Entity entity, bool asServer)
        {
            if (!IsServer || !asServer) return;
            if (!entity.RequiredSystems.Contains("physical-object")) return;
            entity.AddSystem<PhysicalObjectSystem>(CharacterPrefab);
        }
    }
}