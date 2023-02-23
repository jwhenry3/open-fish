using FishNet.Object;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Inventory
{
    public class InventorySystem : EntitySystem
    {
        public override string GetSystemName() => "inventory";
        public Bag Bag;
        public BagUI BagUIPrefab;
        private BagUI BagUI;
        public string MenuHotkey = "Inventory";
        public string CloseHotkey = "Cancel";

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!IsOwner) return;
            // create the BagUI
           var instance = Instantiate(BagUIPrefab.gameObject);
           BagUI = instance.GetComponent<BagUI>();
           BagUI.Initialize(Entity.EntityId);
        }

        protected override void Update()
        {
            base.Update();
            if (BagUI == null) return;
            if (Input.GetButtonDown(MenuHotkey))
            {
                BagUI.gameObject.SetActive(!BagUI.gameObject.activeInHierarchy);
            }

            if (Input.GetButtonDown(CloseHotkey))
            {
                BagUI.gameObject.SetActive(false);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (BagUI == null) return;
            Destroy(BagUI.gameObject);
            BagUI = null;
        }

        public void TakeFromChest(Chest chest, int position)
        {
            var entity = chest.GetComponent<Entity>();
            Server_TakeFromChest(entity.EntityId, position);
        }

        public void PutInChest(Chest chest, int position)
        {
            var entity = chest.GetComponent<Entity>();
            Server_PutInChest(entity.EntityId, position);
        }
        
        [ServerRpc]
        private void Server_TakeFromChest(string chestEntityId, int position)
        {
            var bag = Bag.GetBag(chestEntityId);
            if (bag == null) return;
            var chest = bag as Chest;
            if (chest == null) return;
            chest.TakeItem(position, Entity.EntityId);
        }

        [ServerRpc]
        private void Server_PutInChest(string chestEntityId, int position)
        {
            var bag = Bag.GetBag(chestEntityId);
            if (bag == null) return;
            var chest = bag as Chest;
            if (chest == null) return;
            chest.PutItem(position, Entity.EntityId);
        }
    }
}