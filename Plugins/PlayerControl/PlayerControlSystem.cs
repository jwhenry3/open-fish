using OpenFish.Plugins.Entities;
using OpenFish.Plugins.PhysicalObject;
using TriInspector;
using UnityEngine;

namespace OpenFish.Plugins.PlayerControl
{
    public class PlayerControlSystem : EntitySystem
    {
        public override string GetSystemName() => "player-control";
        
        [Required]
        [Group("manual")]
        public PhysicalObjectSystem ObjectSystem;
        [Required]
        [Group("manual")]
        public PlayerController Controller;

        private void SetControllerObject()
        {
            if (!IsOwner) return;
            Controller.PhysicalObject = ObjectSystem.Object;
            Controller.enabled = true;
            Controller.Initialize();
            Controller.Camera.gameObject.SetActive(true);
        }

        public override void OnEntityReady(bool asServer)
        {
            base.OnEntityReady(asServer);
            if (asServer || !IsOwner) return;
            Controller.enabled = false;
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
        protected override void Reset()
        {
            base.Reset();
            if (!ObjectSystem)
            {
                ObjectSystem = GetComponent<PhysicalObjectSystem>();
            }
        }
    }
}