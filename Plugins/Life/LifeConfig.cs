using System;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Life
{
    [CreateAssetMenu(fileName = "LifeConfig", menuName = "OpenFish/Life")]
    [Serializable]
    public class LifeConfig : EntitySystemConfig
    {
        public float MaxHealth;
    }
}