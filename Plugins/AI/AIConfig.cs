using System;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.AI
{
    [CreateAssetMenu(fileName = "AIConfig", menuName = "OpenFish/AI/Config")]
    [Serializable]
    public class AIConfig : EntitySystemConfig
    {
        public bool Wanders = false;
        public float WanderRadius = 10;
    }
}