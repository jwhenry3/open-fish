using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenFish.Plugins.PlayerControl
{
    public class PlayerInput<T> where T : Enum
    {
        public readonly Dictionary<KeyCode, T> Map = new();
        public readonly Dictionary<T, bool> Pressed = new();

        public virtual void Initialize()
        {
        }
    }
}