using System;
using FishNet;
using FishNet.Object;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Character
{
    public class CharacterManager : NetworkBehaviour
    {
        public NetworkObject CharacterPrefab;
        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkManager.GetInstance<EntityManager>().EntityAdded += AddCharacter;
        }

        private void OnDestroy()
        {
            if (IsServer)
                NetworkManager.GetInstance<EntityManager>().EntityAdded -= AddCharacter;
        }

        void AddCharacter(Entity entity, bool asServer)
        {
            if (!IsServer || !asServer) return;
            var nob = NetworkManager.GetPooledInstantiated(CharacterPrefab, true);
            var t = nob.transform;
            t.parent = entity.transform;
            t.localPosition = Vector3.zero;
            var info = nob.GetComponent<Character.CharacterInfo>();
            info.Name = "" + entity.OwnerId;
            NetworkManager.ServerManager.Spawn(nob, entity.Owner);
        }
    }
}