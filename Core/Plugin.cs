using System;
using FishNet.Object;
using UnityEngine;

namespace OpenFish.Core
{
    public class Plugin : NetworkBehaviour
    {
        public event Action Enabled;
        public string Name;
        public string Description;
        public string[] Dependencies;

        private void Update()
        {
            
        }

        public void Enable()
        {
            Enabled?.Invoke();
        }
    }
}