using FishNet.Object.Synchronizing;

namespace OpenFish.Plugins.Inventory
{
    public class Chest : Bag
    {
        [SyncObject] public readonly SyncList<string> OwningPlayers = new();

        public void TakeItem(int position, string playerEntityId)
        {
            if (!IsServer) return;
            if (!OwningPlayers.Contains(playerEntityId)) return;
            var item = GetItem(position);
            if (item == null) return;
            var bag = GetPlayerBag(playerEntityId);
            if (bag == null) return;
            // actually perform the transaction
            bag.Add(item.ItemId, item.Amount);
            Remove(item.Position);
        }

        public void PutItem(int position, string playerEntityId)
        {
            if (!IsServer) return;
            if (!OwningPlayers.Contains(playerEntityId)) return;
            var bag = GetPlayerBag(playerEntityId);
            if (bag == null) return;
            var item = bag.GetItem(position);
            if (item == null) return;
            // actually perform the transaction
            Add(item.ItemId, item.Amount);
            bag.Remove(item.Position);
        }
    }
}