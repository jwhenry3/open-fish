using FishNet.Object;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Inventory
{
    public class InventorySystem : EntitySystem
    {
        public override string GetSystemName() => "inventory";
        public Bag Bag;

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
    }
}