using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace OpenFish.Plugins.Entities
{
    public class Entity : NetworkBehaviour
    {
        [SyncVar]
        public string EntityId;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            NetworkManager.GetInstance<EntityManager>().AddEntity(this);
        }

        public void OnDestroy()
        {
            NetworkManager.GetInstance<EntityManager>().RemoveEntity(this);
        }
    }
}