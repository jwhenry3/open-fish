using System;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "OpenFish/Level/Config")]
    [Serializable]
    public class LevelConfig : EntitySystemConfig
    {
        public int Level = 1;
    }
}