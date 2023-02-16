using OpenFish.Plugins.Entities;
using TriInspector;

namespace OpenFish.Plugins.Life
{
    public class LifeSystem : EntitySystem
    {
        public override string GetSystemName() => "life";
        [Group("manual")] public float MaxHealth = 100;
        [Group("manual")] public float CurrentHealth = 100;

        public float PerLevel = 10;

        [Group("manual")] public LifeConfig TypeConfig;
        [Group("manual")] public LifeConfig IdConfig;

        public override void OnEntityReady(bool asServer)
        {
            base.OnEntityReady(asServer);

            if (TypeConfig == null && LifeConfigRepo.TypeConfigs.ContainsKey(Entity.EntityType))
                TypeConfig = LifeConfigRepo.TypeConfigs[Entity.EntityType];

            if (IdConfig == null && LifeConfigRepo.IdConfigs.ContainsKey(Entity.EntityId))
                IdConfig = LifeConfigRepo.IdConfigs[Entity.EntityId];

            if (TypeConfig != null)
            {
                MaxHealth = TypeConfig.MaxHealth;
                PerLevel = TypeConfig.PerLevel;
            }

            if (IdConfig != null)
            {
                MaxHealth = IdConfig.MaxHealth;
                PerLevel = IdConfig.PerLevel;
            }

            CurrentHealth = MaxHealth;
        }
    }
}