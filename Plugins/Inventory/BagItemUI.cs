using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OpenFish.Plugins.Inventory
{
    public class BagItemUI : MonoBehaviour, IPointerUpHandler
    {
        public Bag Bag;
        public int Position;
        public ItemAmount Item;
        public Image Image;
        public Action OnClick;

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
                    Image.sprite = repo.Items[Item.ItemId].Graphic;
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
            if (position != Position) return;
            Item = Bag.GetItem(position);
            var repo = InventoryManager.Instance.Repo;
            if (Item == null || !repo.Items.ContainsKey(Item.ItemId))
            {
                Image.sprite = null;
                Image.color = Color.clear;
                return;
            }

            Image.sprite = repo.Items[Item.ItemId].Graphic;
            Image.color = Color.white;
        }

        private void OnDisable()
        {
            Debug.Log("Called Disable!");
            if (Bag != null)
            {
                Bag.ItemUpdated -= OnUpdate;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Invoke!");
            OnClick?.Invoke();
        }
    }
}