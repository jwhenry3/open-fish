using FishNet.Object;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.PhysicalObject
{
    public class PhysicalObjectManager : EntitySystemManager<PhysicalObjectSystem>
    {
        public PhysicalObjectConfigRepo Repo;
    }
}