using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Skill
{
    public class SkillSystem : EntitySystem
    {
        public override string GetSystemName() => "skill";
        [SyncObject] public readonly SyncDictionary<string, float> SkillLevels = new();

        public override void OnStartServer()
        {
            base.OnStartServer();
            // initialize all skill levels to 0
            foreach (var kvp in SkillManager.Instance.Repo.Skills)
            {
                SkillLevels.Add(kvp.Key, 0);
            }
        }
    }
}