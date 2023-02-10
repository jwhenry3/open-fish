using System;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
using OpenFish.Plugins.PhysicalObject;
using UnityEngine;

namespace OpenFish.Plugins.Pet
{
    public class PetOwnerSystem : EntitySystem
    {
        public override string GetSystemName() => "pet-owner";
        private Transform Object;
        private Transform t;
        private float tick;
        public override void OnEntityReady()
        {
            base.OnEntityReady();
            t = transform;
            Object = Entity.GetSystem<PhysicalObjectSystem>().transform;
        }

        private void Update()
        {
            if (t != null && Object != null)
            {
                tick += Time.deltaTime;
                if (!(tick > 1)) return;
                t.position = Object.transform.position;
                tick = 0;
            }
        }
    }
}