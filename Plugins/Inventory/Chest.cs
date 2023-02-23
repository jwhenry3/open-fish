using FishNet.Object.Synchronizing;

namespace OpenFish.Plugins.Inventory
{
    public class Chest : Bag
    {
        public void TakeItem(int position, string playerEntityId)
        {
            if (!IsServer) return;
            var item = GetItem(position);
            if (item == null) return;
            var bag = GetBag(playerEntityId);
            if (bag == null) return;
            // actually perform the transaction
            if (bag.Add(item.ItemId, item.Amount))
                Remove(item.Position);
        }

        public void PutItem(int position, string playerEntityId)
        {
            if (!IsServer) return;
            var bag = GetBag(playerEntityId);
            if (bag == null) return;
            var item = bag.GetItem(position);
            if (item == null) return;
            // actually perform the transaction
            if (Add(item.ItemId, item.Amount))
                bag.Remove(item.Position);
        }
    }
}