using System;

namespace OpenFish.Plugins.Inventory
{
    [Serializable]
    public class ItemAmount
    {
        public int Position;
        public string ItemId;
        public int Amount;
    }
}