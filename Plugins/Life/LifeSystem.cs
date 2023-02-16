using OpenFish.Plugins.Entities;
using TriInspector;

namespace OpenFish.Plugins.Life
{
    public class LifeSystem : EntitySystem
    {
        public override string GetSystemName() => "life";
        [Group("manual")]
        public float MaxHealth = 100;
        [Group("manual")]
        public float CurrentHealth = 100;

    }
}