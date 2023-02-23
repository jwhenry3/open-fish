using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Inventory
{
    public class Bag : NetworkBehaviour
    {
        public int Capacity = 30;
        [SyncObject]
        public readonly SyncDictionary<int, ItemAmount> Items = new();

        public List<ItemAmount> StartingItems;

        public static Bag GetPlayerBag(string playerEntityId)
        {
            var entity = EntityManager.GetEntity(playerEntityId);
            if (entity == null) return null;
            if (entity.EntityType != "player") return null;
            var inventory = entity.GetSystem<InventorySystem>();
            if (inventory == null) return null;
            return inventory.Bag;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            if (StartingItems is not { Count: > 0 }) return;
            foreach (var item in StartingItems)
                Add(item.ItemId, item.Amount);
        }

        [ServerRpc]
        public void Client_Split(int position, int amount)
        {
            Split(position, amount);
        }

        [ServerRpc]
        public void Client_Move(int oldPosition, int newPosition)
        {
            Move(oldPosition, newPosition);
        }
        
        
        public void Add(string itemId, int amount)
        {
            if (!IsServer) return;
            var existing = FirstItemByItemId(itemId);
            if (existing != null)
            {
                Items[existing.Position] = new ItemAmount()
                {
                    Position = existing.Position,
                    ItemId = itemId,
                    Amount = existing.Amount + amount
                };
                return;
            }

            var empty = FirstEmptyPosition();
            if (empty == -1) return;
            Items[empty] = new ItemAmount()
            {
                Position = empty,
                ItemId = itemId,
                Amount = amount
            };
        }

        public void Split(int position, int amount)
        {
            if (!IsServer) return;
            var item = GetItem(position);
            if (item == null) return;
            if (item.Amount <= amount) return;
            Items[position] = new ItemAmount()
            {
                Position = position,
                ItemId = item.ItemId,
                Amount = item.Amount - amount
            };
            for (var pos = 0; pos < Capacity; pos++)
            {
                if (Items.ContainsKey(pos)) continue;
                Items[pos] = new ItemAmount()
                {
                    Position = pos,
                    ItemId = item.ItemId,
                    Amount = amount
                };
                break;
            }
        }

        public void Move(int oldPosition, int newPosition)
        {
            if (!IsServer) return;
            var firstItem = GetItem(oldPosition);
            var secondItem = GetItem(newPosition);
            if (secondItem != null && firstItem != null)
            {
                if (secondItem.ItemId == firstItem.ItemId)
                {
                    Items.Remove(oldPosition);
                    Items[newPosition] = new ItemAmount()
                    {
                        Position = newPosition,
                        ItemId = secondItem.ItemId,
                        Amount = firstItem.Amount + secondItem.Amount
                    };
                    return;
                }
            }

            if (firstItem != null)
            {
                Items[newPosition] = new ItemAmount()
                {
                    Position = newPosition,
                    ItemId = firstItem.ItemId,
                    Amount = firstItem.Amount
                };
            }

            if (secondItem != null)
            {
                Items[oldPosition] = new ItemAmount()
                {
                    Position = oldPosition,
                    ItemId = secondItem.ItemId,
                    Amount = secondItem.Amount
                };
            }
            else
            {
                Remove(oldPosition);
            }
        }

        public void Remove(int position)
        {
            if (!IsServer) return;
            if (Items.ContainsKey(position))
                Items.Remove(position);
        }

        public int FirstEmptyPosition()
        {
            for (var pos = 0; pos < Capacity; pos++)
            {
                if (Items.ContainsKey(pos)) continue;
                return pos;
            }

            return -1;
        }

        public ItemAmount FirstItemByItemId(string itemId)
        {
            return Items.Where(kvp => kvp.Value.ItemId == itemId).Select(kvp => kvp.Value).FirstOrDefault();
        }

        public List<ItemAmount> FindItemsByItemId(string itemId)
        {
            return (from kvp in Items where kvp.Value.ItemId == itemId select kvp.Value).ToList();
        }

        public ItemAmount GetItem(int position)
        {
            return Items.ContainsKey(position) ? Items[position] : null;
        }
    }
}