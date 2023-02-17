using System;
using UnityEngine;

namespace OpenFish.Plugins.Skill
{
    [CreateAssetMenu(fileName = "SkillConfig", menuName = "OpenFish/Skill/Config")]
    [Serializable]
    public class SkillConfig : ScriptableObject
    {
        public string Id;
        public string Name;
    }
}