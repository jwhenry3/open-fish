using System;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Pet
{
    public class PetSystem : EntitySystem
    {
        public override string GetSystemName()
        {
            return "pet";
        }
    }
}