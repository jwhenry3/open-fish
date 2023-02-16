using OpenFish.Plugins.Entities;
using TriInspector;

namespace OpenFish.Plugins.AI
{
    [DeclareTabGroup("behaviors")]
    public class AISystem : EntitySystem
    {
        public override string GetSystemName() => "ai";
        
        [Group("derived")]
        public AIConfig TypeConfig;
        [Group("derived")]
        public AIConfig IdConfig;
        
        [Group("behaviors"), Tab("Wandering")]
        public bool Wanders = false;
        [Group("behaviors"), Tab("Wandering")]
        public float WanderRadius = 10;
        [Group("behaviors"), Tab("Chasing")]
        public bool Chases = false;
        [Group("behaviors"), Tab("Chasing")]
        public float ChaseDistance = 10;

        public override void OnEntityReady(bool asServer)
        {
            base.OnEntityReady(asServer);
            if (!asServer) return;
            if (AIConfigRepo.TypeConfigs.ContainsKey(Entity.EntityType))
            {
                TypeConfig = AIConfigRepo.TypeConfigs[Entity.EntityType];
                
            }

            if (AIConfigRepo.IdConfigs.ContainsKey(Entity.EntityId))
            {
                IdConfig = AIConfigRepo.IdConfigs[Entity.EntityId];
                
            }
        }
    }
}