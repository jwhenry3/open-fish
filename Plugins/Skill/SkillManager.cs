using System;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Skill
{
    public class SkillManager : EntitySystemManager<SkillSystem>
    {
        public static event Action<SkillManager> Instantiated;
        public static SkillManager Instance;

        public SkillConfigRepo Repo;
        
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            Repo.OnStart();
            Instantiated?.Invoke(this);
        }
    }
}