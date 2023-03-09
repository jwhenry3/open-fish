using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Inventory
{
    public class InventoryManager : EntitySystemManager<InventorySystem>
    {
        public ItemRepo Repo;
        public static InventoryManager Instance;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            Repo.OnStart();
        }
    }
}