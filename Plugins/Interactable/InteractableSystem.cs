using System;
using System.Collections.Generic;
using OpenFish.Plugins.Entities;
using UnityEngine;

namespace OpenFish.Plugins.Interactable
{
    public class InteractableSystem : EntitySystem
    {
        public float VisibleRadius;
        public float InteractableRadius;
        public SphereCollider VisibleCollider;
        public SphereCollider InteractableCollider;
        private readonly Dictionary<string, int> Counts = new();

        private void Awake()
        {
            if (!VisibleCollider)
            {
                var component = gameObject.AddComponent<SphereCollider>();
                component.radius = VisibleRadius;
                VisibleCollider = component;
            }

            if (!InteractableCollider)
            {
                var component = gameObject.AddComponent<SphereCollider>();
                component.radius = InteractableRadius;
                InteractableCollider = component;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var obj = other.GetComponent<PhysicalObject.PhysicalObject>();
            if (obj == null) return;
            if (!Counts.ContainsKey(obj.EntityId))
                Counts[obj.EntityId] = 0;
            Counts[obj.EntityId]++;
        }

        private void OnTriggerExit(Collider other)
        {
            var obj = other.GetComponent<PhysicalObject.PhysicalObject>();
            if (obj == null) return;
            if (!Counts.ContainsKey(obj.EntityId))
                Counts[obj.EntityId] = 0;
            if (Counts[obj.EntityId] > 0)
                Counts[obj.EntityId]--;
        }
    }
}