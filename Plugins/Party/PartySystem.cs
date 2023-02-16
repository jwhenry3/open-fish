using System;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Pet
{
    public class PartySystem : EntitySystem
    {
        public override string GetSystemName() => "party";
        [SyncVar] public bool IsLeader;
        [SyncVar] public string LeaderId;
        [SyncObject] public readonly SyncList<string> Members = new();
        [SyncObject] public readonly SyncList<string> Invites = new();
        [SyncObject] public readonly SyncList<string> JoinRequests = new();

    }
}