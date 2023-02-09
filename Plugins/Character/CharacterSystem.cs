using FishNet.Object;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Character
{
    public class CharacterSystem : EntitySystem
    {
        public new string SystemName = "character";
        [SyncVar] public string Name;
        
        public GameObject PhysicalObject;

        public override void OnEntityReady()
        {
            base.OnEntityReady();
            if (IsServer)
            {
                if (CharacterConfig.IdConfigs.ContainsKey(Entity.EntityId))
                    Name = CharacterConfig.IdConfigs[Entity.EntityId].Name;
            }
            Debug.Log("The Entity Is Ready!");
        }
    }
}