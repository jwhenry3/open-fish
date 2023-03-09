using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.AI
{
    public class AIManager : EntitySystemManager<AISystem>
    {
        public AIConfigRepo Repo;

        protected override void Awake()
        {
            base.Awake();
            Repo.OnStart();
        }
    }
}