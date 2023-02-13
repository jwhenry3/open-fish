using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.AI
{
    public class AISystem : EntitySystem
    {
        public override string GetSystemName() => "ai";
        
        public AIConfig TypeConfig;
        public AIConfig IdConfig;

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