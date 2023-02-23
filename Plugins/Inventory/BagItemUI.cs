using System;
using UnityEngine;
using UnityEngine.UI;

namespace OpenFish.Plugins.Inventory
{
    public class BagItemUI : MonoBehaviour
    {
        public Bag Bag;
        public int Position;
        public ItemAmount Item;
        public Image Image;

        public void Initialize(Bag bag, int position)
        {
            Bag = bag;
            Position = position;
            Bag.ItemUpdated += OnUpdate;
            Item = Bag.GetItem(position);
            Image.sprite = null;
            Image.color = Color.clear;
            if (Item != null)
            {
                var repo = InventoryManager.Instance.Repo;
                if (repo.Items.ContainsKey(Item.ItemId))
                {
                    Image.sprite =  repo.Items[Item.ItemId].Graphic;
                    Image.color = Color.white;
                }
            }
        }

        private void OnEnable()
        {
            if (Bag != null)
            {
                Bag.ItemUpdated += OnUpdate;
            }
        }

        private void OnUpdate(int position)
        {
            if (position == Position)
            {
                Item = Bag.GetItem(position);
            }
        }

        private void OnDisable()
        {
            Debug.Log("Called Disable!");
        }
    }
}