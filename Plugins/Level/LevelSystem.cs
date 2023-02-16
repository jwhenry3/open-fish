using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Level
{
    public class LevelSystem : EntitySystem
    {
        public override string GetSystemName() => "level";
        public int Level = 1;

        public override void OnEntityReady(bool asServer)
        {
            base.OnEntityReady(asServer);
        }
    }
}