using System;
using UnityEngine;
using UnityEngine.UI;

namespace OpenFish.Plugins.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "OpenFish/Inventory/Item")]
    [Serializable]
    public class Item : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Description;
        public Sprite Graphic;
        public bool Stackable;
    }
}