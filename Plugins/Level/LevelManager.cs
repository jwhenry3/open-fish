using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Level
{
    public class LevelManager : EntitySystemManager<LevelSystem>
    {
        public LevelConfigRepo Repo;

        protected override void Awake()
        {
            base.Awake();
            Repo.OnStart();
        }
    }
}