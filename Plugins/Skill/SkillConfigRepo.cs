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

        private void OnEnable()
        {
            Skills = new();
            foreach (var guid in AssetDatabase.FindAssets("t:SkillConfig"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(SkillConfig)) as SkillConfig;
                if (asset == null) continue;
                Skills.Add(asset.Id, asset);
            }
            Debug.Log("Found " + Skills.Count + " Configs for Skills");
        }
    }
}