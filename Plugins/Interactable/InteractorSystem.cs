using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using OpenFish.Plugins.Entities;
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
                if (_transform.position != lastLocation)
                {
                    Interrupt();
                }
                else
                {
                    interactCurrentDuration += Time.deltaTime;
                    if (interactCurrentDuration >= CurrentInteraction.InteractDuration)
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
            var entity = interactable.GetComponent<Entity>();
            if (entity == null) return;
            Server_Interact_With(entity.EntityId);
            Debug.Log("Interacting with " + entity.Name + "(" + entity.EntityId + ")");
        }

        [ServerRpc]
        private void Server_Interact_With(string entityId)
        {
            var entity = EntityManager.GetEntity(entityId);
            if (entity == null) return;
            var interactable = entity.GetComponent<InteractableSystem>();
            if (interactable == null) return;
            if (Vector3.Distance(
                    _transform.position,
                    interactable._transform.position
                ) > interactable.InteractableRadius) return;
            CurrentInteraction = interactable;
            IsInteracting = true;
            lastLocation = _transform.position;
        }

        public void Interrupt()
        {
            if (!IsServer) return;
            Interrupted?.Invoke();
            Stop();
            Client_Interrupted();
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
            Stop();
            Client_Completed();
        }

        [ObserversRpc]
        private void Client_Completed()
        {
            if (!IsOwner) return;
            Interacted?.Invoke(CurrentInteraction);
            CurrentInteraction = null;
            Debug.Log("Interaction Complete!");
        }

        [ObserversRpc]
        private void Client_Interrupted()
        {
            if (!IsOwner) return;
            Interrupted?.Invoke();
            CurrentInteraction = null;
            Debug.Log("Interaction Interrupted!");
        }
    }
}