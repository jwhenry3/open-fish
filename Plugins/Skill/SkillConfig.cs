using System;
using UnityEngine;

namespace OpenFish.Plugins.Skill
{
    [CreateAssetMenu(fileName = "SkillConfig", menuName = "OpenFish/Skill/Config")]
    [Serializable]
    public class SkillConfig : ScriptableObject
    {
        // Short constant name to reference skills by
        public string Id;
        // Use Category to group skills in a sane way, in order to
        // display certain skills
        public string Category;
        // More descriptive name
        public string Name;
        public string Description;
    }
}