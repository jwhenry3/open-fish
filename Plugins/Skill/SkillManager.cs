using System;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Skill
{
    public class SkillManager : EntitySystemManager<SkillSystem>
    {
        public static event Action<SkillManager> Instantiated;
        public static SkillManager Instance;

        public SkillConfigRepo Repo;
        
        private void Awake()
        {
            Instance = this;
            Instantiated?.Invoke(this);
        }
    }
}