using FishNet.Object;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Inventory
{
    public class InventorySystem : EntitySystem
    {
        public override string GetSystemName() => "inventory";
        public Bag Bag;
        public BagUI BagUIPrefab;
        private BagUI BagUI;

        [ServerRpc]
        public void TakeItemFromChest(Chest chest, int position)
        {
            var entityId = Entity.EntityId;
            chest.TakeItem(position, entityId);
        }
        
        [ServerRpc]
        public void PutItemInChest(Chest chest, int position)
        {
            var entityId = Entity.EntityId;
            chest.PutItem(position, entityId);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!IsOwner) return;
            // create the BagUI
           var instance = Instantiate(BagUIPrefab.gameObject);
           BagUI = instance.GetComponent<BagUI>();
           BagUI.Initialize(Entity.EntityId);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (BagUI == null) return;
            Destroy(BagUI.gameObject);
            BagUI = null;
        }
    }
}