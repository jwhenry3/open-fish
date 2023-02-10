using System;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Pet
{
    public class PetOwnerSystem : EntitySystem
    {
        public override string GetSystemName() => "pet-owner";
    }
}