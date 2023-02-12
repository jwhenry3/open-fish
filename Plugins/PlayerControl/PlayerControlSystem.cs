using OpenFish.Plugins.Entities;
using OpenFish.Plugins.PhysicalObject;
using UnityEngine;

namespace OpenFish.Plugins.PlayerControl
{
    public class PlayerControlSystem : EntitySystem
    {
        public override string GetSystemName() => "player-control";
        
        public PhysicalObjectSystem ObjectSystem;
        public PlayerController Controller;

        private void SetControllerObject()
        {
            if (!IsOwner) return;
            Controller.ObjectTransform = ObjectSystem.Object;
            foreach (Transform child in ObjectSystem.Object)
            {
                var cam = child.GetComponent<Camera>();
                if (cam == null) continue;
                Controller._camera = cam.gameObject;
                Controller._camera.SetActive(true);
                break;
            }
        }

        public override void OnEntityReady(bool asServer)
        {
            base.OnEntityReady(asServer);
            if (asServer || !IsOwner) return;
            // We only do this on the client since the client is the only one that cares
            ObjectSystem = Entity.GetSystem<PhysicalObjectSystem>();
            if (ObjectSystem.Object)
                SetControllerObject();
            else
                ObjectSystem.ObjectInstantiated += SetControllerObject;
        }

        protected override void OnDestroy()
        {
            if (ObjectSystem != null)
                ObjectSystem.ObjectInstantiated -= SetControllerObject;
        }
    }
}