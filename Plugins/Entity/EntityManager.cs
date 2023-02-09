using FishNet.Object;

namespace OpenFish.Plugins.Entity
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