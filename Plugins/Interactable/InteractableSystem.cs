using System.Collections.Generic;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Interactable
{
    public class InteractableSystem : EntitySystem
    {
        public override string GetSystemName() => "interactable";
        public static readonly List<InteractableSystem> Interactables = new();
        public static List<InteractableSystem> Sorted = new();
        public float InteractDuration = 1;
        public bool CanInteract = true;
        public Transform Canvas;
        public GameObject VisibleObject;
        public GameObject InteractObject;
        public float VisibleRadius = 6;
        public float InteractableRadius = 2;
        public SphereCollider VisibleCollider;
        public SphereCollider InteractableCollider;
        private int count;
        private Camera _camera;
        private Transform _cameraTransform;
        private PhysicalObject.PhysicalObject _physicalObject;
        private Entity _entity;
        public Transform _transform;

        private void Awake()
        {
            _transform = transform;
            if (!VisibleCollider)
                VisibleCollider = gameObject.AddComponent<SphereCollider>();

            if (!InteractableCollider)
                InteractableCollider = gameObject.AddComponent<SphereCollider>();

            VisibleCollider.isTrigger = true;
            VisibleCollider.radius = VisibleRadius;
            InteractableCollider.isTrigger = true;
            InteractableCollider.radius = InteractableRadius;
        }

        protected override void Update()
        {
            base.Update();
            UpdateVisibility();
        }

        private Entity GetLocalPlayer(Collider other)
        {
            var obj = other.GetComponent<PhysicalObject.PhysicalObject>();
            if (obj == null) return null;
            if (!EntityManager.Entities.ContainsKey(obj.EntityId)) return null;
            var entity = EntityManager.Entities[obj.EntityId];
            if (entity == null || entity.EntityType != "player") return null;
            if (!entity.IsOwner || !entity.IsClient) return null;
            _camera = obj.Camera;
            _cameraTransform = _camera.transform;
            _entity = entity;
            _physicalObject = obj;
            return entity;
        }

        private void OnTriggerEnter(Collider other)
        {
            var entity = GetLocalPlayer(other);
            if (entity == null) return;

            count++;
            if (count > 1 && !Interactables.Contains(this))
                Interactables.Add(this);
            UpdateVisibility();
        }

        private void OnTriggerExit(Collider other)
        {
            var entity = GetLocalPlayer(other);
            if (entity == null) return;
            if (count > 1 && Interactables.Contains(this))
                Interactables.Remove(this);
            count--;
            count = count < 0 ? 0 : count;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            if (_cameraTransform != null)
            {
                Canvas.forward = _cameraTransform.forward;
            }

            if (count > 0 != VisibleObject.activeSelf)
                VisibleObject.SetActive(count > 0);
            var isClosest = Sorted.Count > 0 && (Sorted[0] == this);
            if (InteractObject.activeSelf != isClosest)
                InteractObject.SetActive(isClosest);
            CanInteract = isClosest;
        }
    }
}