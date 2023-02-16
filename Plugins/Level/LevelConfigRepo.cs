using System;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Level
{
    [CreateAssetMenu(fileName = "LevelConfigRepo", menuName = "OpenFish/Level/Repo")]
    [Serializable]
    public class LevelConfigRepo : EntitySystemConfigRepo<LevelConfig>
    {
        
    }
}