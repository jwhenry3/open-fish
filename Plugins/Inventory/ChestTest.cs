using UnityEngine;

namespace OpenFish.Plugins.Inventory
{
    public class ChestTest : MonoBehaviour
    {
        public Chest Chest;
        public string ChestId;

        public void AddCarrot(string playerEntityId, bool asServer)
        {
            if (!asServer) return;
            var bag = Bag.GetPlayerBag(playerEntityId);
            if (bag == null) return;
            if (!bag.Add("carrot", 1))
            {
                Debug.Log("No Room");
                return;
            }
            var item = bag.GetItem(0);
            if (item == null) return;
            Debug.Log("Player now has " + item.Amount + " Carrots!");
        }
    }
}