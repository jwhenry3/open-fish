using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Life
{
    public class LifeManager : EntitySystemManager<LifeSystem>
    {
        public LifeConfigRepo Repo;

        protected override void Awake()
        {
            base.Awake();
            Repo.OnStart();
        }
    }
}