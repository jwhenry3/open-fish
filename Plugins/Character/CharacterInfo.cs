using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace OpenFish.Plugins.Character
{
    public class CharacterInfo : NetworkBehaviour
    {
        [SyncVar]
        public string Name;

        [SyncVar]
        public float Count;

        private void Update()
        {
            if (IsServer)
            {
                Count += Time.deltaTime;
            }
        }
    }
}