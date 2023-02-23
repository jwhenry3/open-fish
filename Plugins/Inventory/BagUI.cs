using UnityEngine;

namespace OpenFish.Plugins.Inventory
{
    public class BagUI : MonoBehaviour
    {
        public string EntityId;

        public Transform GridContainer;
        public BagItemUI ItemPrefab;

        private Bag Bag;
        
        public void Initialize(string entityId)
        {
            // no changes needed
            if (Bag != null && entityId == EntityId) return;
            if (string.IsNullOrEmpty(entityId)) return;
            // clean the UI
            foreach (Transform child in GridContainer)
                Destroy(child.gameObject);
            EntityId = entityId;
            
            Bag = Bag.GetPlayerBag(entityId);
            if (Bag == null) return;
            // update the bag UI
            for (var i = 0; i < Bag.Capacity; i++)
                CreateBagItemUI(i);
        }

        private void CreateBagItemUI(int position)
        {
            var instance = Instantiate(ItemPrefab.gameObject, GridContainer);
            var itemUI = instance.GetComponent<BagItemUI>();
            itemUI.Initialize(Bag, position);
        }
    }
}