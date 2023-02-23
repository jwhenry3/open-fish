using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Inventory
{
    public class ChestUI : MonoBehaviour
    {
        public Chest Chest;
        public BagUI ChestBagUI;
        public BagUI PlayerBagUI;

        public void Initialize(string playerEntityId)
        {
            var entity = GetComponentInParent<Entity>();
            ChestBagUI.Initialize(entity.EntityId);
            PlayerBagUI.Initialize(playerEntityId);
        }

        public void TakeItem(BagItemUI item)
        {
            Debug.Log("Take Item");
            var inventory = EntityManager.GetEntity(PlayerBagUI.EntityId).GetSystem<InventorySystem>();
            inventory.TakeFromChest(Chest, item.Position);
        }

        public void PutItem(BagItemUI item)
        {
            Debug.Log("Put Item");
            var inventory = EntityManager.GetEntity(PlayerBagUI.EntityId).GetSystem<InventorySystem>();
            inventory.PutInChest(Chest, item.Position);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}