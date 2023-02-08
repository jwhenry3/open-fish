using FishNet.Object;

namespace OpenFish.Core
{
    public class EntityManager : NetworkBehaviour
    {
        
        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
        }
    }
}