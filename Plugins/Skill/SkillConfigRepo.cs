using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenFish.Plugins.Skill
{
    [CreateAssetMenu(fileName = "SkillConfigRepo", menuName = "OpenFish/Skill/Repo")]
    [Serializable]
    public class SkillConfigRepo : ScriptableObject
    {
        public Dictionary<string, SkillConfig> Skills;
        public Dictionary<string, List<SkillConfig>> SkillsByCategory;

        private void OnEnable()
        {
            Skills = new();
            SkillsByCategory = new();
            foreach (var guid in AssetDatabase.FindAssets("t:SkillConfig"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(SkillConfig)) as SkillConfig;
                if (asset == null) continue;
                if (!SkillsByCategory.ContainsKey(asset.Category))
                    SkillsByCategory[asset.Category] = new();
                Skills.Add(asset.Id, asset);
                SkillsByCategory[asset.Category].Add(asset);
            }
            Debug.Log("Found " + Skills.Count + " Configs for Skills");
        }
    }
}