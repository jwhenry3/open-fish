using System;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Life
{
    [CreateAssetMenu(fileName = "LifeConfigRepo", menuName = "OpenFish/Life/Repo")]
    [Serializable]
    public class LifeConfigRepo : EntitySystemConfigRepo<LifeConfig>
    {
    }
}