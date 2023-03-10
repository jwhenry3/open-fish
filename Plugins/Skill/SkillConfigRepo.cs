using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OpenFish.Plugins.Skill
{
    [CreateAssetMenu(fileName = "SkillConfigRepo", menuName = "OpenFish/Skill/Repo")]
    [Serializable]
    public class SkillConfigRepo : ScriptableObject
    {
        public bool UseExamples;
        public Dictionary<string, SkillConfig> Skills;
        public Dictionary<string, List<SkillConfig>> SkillsByCategory;

        public void OnStart()
        {
#if UNITY_EDITOR
            Skills = new();
            SkillsByCategory = new();
            foreach (var guid in AssetDatabase.FindAssets("t:SkillConfig"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (UseExamples && !path.Contains("OpenFish/Examples"))
                    continue;
                if (!UseExamples && path.Contains("OpenFish/Examples"))
                    continue;
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(SkillConfig)) as SkillConfig;
                if (asset == null) continue;
                if (asset.Category != null)
                {
                    if (!SkillsByCategory.ContainsKey(asset.Category))
                        SkillsByCategory[asset.Category] = new();
                    SkillsByCategory[asset.Category].Add(asset);
                }

                Skills.Add(asset.Id, asset);
            }
            Debug.Log("Found " + Skills.Count + " Config(s) for Skills");
#endif
        }
    }
}