using System;
using UnityEngine;

namespace OpenFish.Plugins.PlayerControl
{
    public class DirectionalInput : PlayerInput<Direction>
    {
        public Vector2 DirectionVector;
        public string Horizontal;
        public string Vertical;
        public string Modifier;
        public float Speed = 1;


        public void Update()
        {
            if (!string.IsNullOrEmpty(Modifier))
            {
                if (!Input.GetButton(Modifier))
                {
                    DirectionVector = new Vector2();
                    return;
                }
            }
            DirectionVector = new Vector2(
                Input.GetAxis(Horizontal),
                Input.GetAxis(Vertical)
            );
        }

        public Vector3 ToMovementVector()
        {
            return new Vector3(
                DirectionVector.x,
                0,
                DirectionVector.y
            );
        }
    }
}