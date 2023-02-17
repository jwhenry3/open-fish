using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
using OpenFish.Plugins.PhysicalObject;
using TriInspector;

namespace OpenFish.Plugins.Level
{
    public class LevelSystem : EntitySystem
    {
        public override string GetSystemName() => "level";
        [Group("manual")]  [SyncVar] public int Level = 1;
        [Group("manual")]  [SyncVar] public float Experience = 0;
        [Group("manual")] public LevelConfig TypeConfig;
        [Group("manual")] public LevelConfig IdConfig;

        public override void OnEntityReady(bool asServer)
        {
            base.OnEntityReady(asServer);

            if (TypeConfig == null && LevelConfigRepo.TypeConfigs.ContainsKey(Entity.EntityType))
                TypeConfig = LevelConfigRepo.TypeConfigs[Entity.EntityType];

            if (IdConfig == null && LevelConfigRepo.IdConfigs.ContainsKey(Entity.EntityId))
                IdConfig = LevelConfigRepo.IdConfigs[Entity.EntityId];

            if (TypeConfig != null)
                Level = TypeConfig.Level;

            if (IdConfig != null)
                Level = IdConfig.Level;
        }
    }
}