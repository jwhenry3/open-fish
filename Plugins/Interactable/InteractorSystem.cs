using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
using OpenFish.Plugins.PhysicalObject;
using UnityEngine;

namespace OpenFish.Plugins.Interactable
{
    public class InteractorSystem : EntitySystem
    {
        private Transform _transform;
        public event Action<InteractableSystem> Interacted;
        public event Action Interrupted;
        public override string GetSystemName() => "interactor";

        [SyncVar] public bool CanInteractWithWorld = true;
        [SyncVar] public float interactCooldown = 0;
        [SyncVar] public float interactCurrentDuration = 0;
        private InteractableSystem CurrentInteraction;
        [SyncVar] public bool IsInteracting;
        private Vector3 lastLocation;

        public bool DisplayLogs;


        private void Awake()
        {
            _transform = transform;
        }

        protected override void Update()
        {
            base.Update();
            if (!IsServer) return;
            if (CurrentInteraction && IsInteracting)
            {
                // Interrupt the action if the player moves
                var obj = Entity.GetSystem<PhysicalObjectSystem>().Object;
                if (obj.position != lastLocation)
                {
                    Interrupt();
                }
                else
                {
                    interactCurrentDuration += Time.deltaTime;
                    if (interactCurrentDuration >= CurrentInteraction.InteractDuration || interactCurrentDuration == 0)
                        Complete();
                }
            }

            lastLocation = _transform.position;
            if (interactCooldown > 0) interactCooldown -= Time.deltaTime;
        }

        public void Interact()
        {
            if (interactCooldown > 0)
            {
                Debug.Log("Cannot do that yet");
                return;
            }

            if (!CanInteractWithWorld) return;
            var interactable = InteractableSystem.Sorted.Count > 0 ? InteractableSystem.Sorted[0] : null;
            if (interactable == null || !interactable.CanInteract) return;
            var entity = interactable.Entity;
            if (entity == null) return;
            Server_Interact_With(entity.EntityId);
        }
        
        [ServerRpc]
        private void Server_Interact_With(string entityId)
        {
            var entity = EntityManager.GetEntity(entityId);
            if (entity == null) return;
            var interactable = entity.GetSystem<InteractableSystem>();
            if (interactable == null) return;
            var obj = Entity.GetSystem<PhysicalObjectSystem>().Object;
            var collider = obj.GetComponent<CapsuleCollider>();
            if (Vector3.Distance(
                    obj.position,
                    interactable._transform.position
                ) > interactable.InteractableRadius + collider.radius) return;
            CurrentInteraction = interactable;
            IsInteracting = true;
            lastLocation = obj.position;
            if (DisplayLogs)
                Debug.Log("Interacting...");
        }

        public void Interrupt()
        {
            if (!IsServer) return;
            Interrupted?.Invoke();
            Stop();
            Client_Interrupted();
            if (DisplayLogs)
                Debug.Log("Interaction Interrupted!");
        }

        private void Stop()
        {
            IsInteracting = false;
            interactCooldown = 1;
            interactCurrentDuration = 0;
            CurrentInteraction = null;
        }

        public void Complete()
        {
            if (!IsServer) return;
            Interacted?.Invoke(CurrentInteraction);
            CurrentInteraction.OnInteracted.Invoke(Entity.EntityId, true);
            Client_Completed(CurrentInteraction.Entity.EntityId);
            Stop();
            if (DisplayLogs)
                Debug.Log("Interaction Complete!");
        }

        [ObserversRpc]
        private void Client_Completed(string interactableEntityId)
        {
            if (!IsOwner) return;
            var interactableEntity = EntityManager.GetEntity(interactableEntityId);
            var interactable = interactableEntity.GetSystem<InteractableSystem>();
            interactable.OnInteracted.Invoke(Entity.EntityId, false);
        }

        [ObserversRpc]
        private void Client_Interrupted()
        {
            if (!IsOwner) return;
            Interrupted?.Invoke();
        }
    }
}