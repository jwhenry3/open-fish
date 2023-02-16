using System;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.AI
{
    [CreateAssetMenu(fileName = "AIConfigRepo", menuName = "OpenFish/AI/Repo")]
    [Serializable]
    public class AIConfigRepo : EntitySystemConfigRepo<AIConfig>
    {
    }
}