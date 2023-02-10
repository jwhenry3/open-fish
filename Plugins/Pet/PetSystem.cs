using System;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Pet
{
    public class PetSystem : EntitySystem
    {
        public override string GetSystemName() => "pet";
        [SyncVar]
        public string OwnerEntityId;
    }
}