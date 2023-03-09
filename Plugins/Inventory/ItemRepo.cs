using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenFish.Plugins.Inventory
{
    
    [CreateAssetMenu(fileName = "ItemRepo", menuName = "OpenFish/Inventory/ItemRepo")]
    [Serializable]
    public class ItemRepo : ScriptableObject
    {
        public bool UseExamples;
        public Dictionary<string, Item> Items;
        
        public void OnStart()
        {
#if UNITY_EDITOR
            Items = new();
            foreach (var guid in AssetDatabase.FindAssets("t:Item"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (UseExamples && !path.Contains("OpenFish/Examples"))
                    continue;
                if (!UseExamples && path.Contains("OpenFish/Examples"))
                    continue;
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(Item)) as Item;
                if (asset == null) continue;

                Items.Add(asset.Id, asset);
            }
            Debug.Log("Found " + Items.Count + " Item(s)");
#endif
        }
    }
}