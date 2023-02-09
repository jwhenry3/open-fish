using System.Linq;
using FishNet.Object;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Character
{
    public class CharacterManager : NetworkBehaviour
    {
        public NetworkObject CharacterPrefab;

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
            if (!entity.RequiredSystems.Contains("character")) return;
           entity.AddSystem<CharacterSystem>(CharacterPrefab);
        }
    }
}