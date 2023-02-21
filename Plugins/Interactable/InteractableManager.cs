using System.Linq;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Interactable
{
    public class InteractableManager : EntitySystemManager<InteractableSystem>
    {
        private float updateTick = 0;
        private void Update()
        {
            if (!IsClient) return;
            if (updateTick > 0.25f)
            {
                updateTick = 0;
                if (Entity.LocalPlayer == null ||
                    !PhysicalObject.PhysicalObject.Objects.ContainsKey(Entity.LocalPlayer.EntityId)) return;
                var obj = PhysicalObject.PhysicalObject.Objects[Entity.LocalPlayer.EntityId];
                var playerTransform = obj._transform;
                InteractableSystem.Sorted = InteractableSystem.Interactables.OrderBy(x =>
                    Vector3.Distance(x._transform.position, playerTransform.position)).ToList();
            }

            updateTick += Time.deltaTime;
        }
    }
}