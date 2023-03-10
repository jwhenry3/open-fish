using System;
using OpenFish.Plugins.Entities;
using OpenFish.Plugins.Interactable;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace OpenFish.Plugins.PlayerControl
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum CameraMovement
    {
        Left,
        Right,
        Up,
        Down,
    }

    [DeclareBoxGroup("manual", Title = "Configurable")]
    public class PlayerController : MonoBehaviour
    {
        private Entity Entity;
        private Rigidbody _rigidbody;
        [HideInInspector]
        public Camera Camera;
        private Transform _cameraTransform;
        private Transform _cameraParent;
        private Transform t;
        [HideInInspector]
        public Transform PhysicalObject;
        private PhysicalObject.PhysicalObject _container;
        private InteractorSystem interactor;

        public bool HasControl = true;

        [Group("manual")]
        public float speed = 30;
        [Group("manual")]
        public float cameraSpeed = 120;


        private readonly DirectionalInput MoveInput = new();
        private readonly DirectionalInput CameraInput = new();
        private readonly DirectionalInput MouseCameraInput = new();

        private float _pitch;
        private float _yaw;

        public string InteractAction = "Interact";

        // Start is called before the first frame update
        private void Awake()
        {
            interactor = GetComponent<InteractorSystem>();
            MoveInput.Horizontal = "Horizontal";
            MoveInput.Vertical = "Vertical";
            CameraInput.Horizontal = "Look X";
            CameraInput.Vertical = "Look Y";
            MouseCameraInput.Horizontal = "Mouse X";
            MouseCameraInput.Vertical = "Mouse Y";
            MouseCameraInput.Modifier = "MouseLook";
            MouseCameraInput.Speed = 2;
        }

        public void Initialize()
        {
            t = PhysicalObject.transform;
            _container = PhysicalObject.GetComponent<PhysicalObject.PhysicalObject>();
            _rigidbody = PhysicalObject.GetComponent<Rigidbody>();
            SetCamera();
        }

        private void SetCamera()
        {
            Camera = Camera.main;
            if (Camera != null)
            {
                _container.Camera = Camera;
                _cameraTransform = Camera.transform;
                _cameraParent = _container.CameraHolder;
                _cameraTransform.parent = _cameraParent;
                _cameraTransform.localPosition = new Vector3(0, 2, -10);
                _cameraTransform.rotation = Quaternion.identity;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Camera == null) return;
            if (!HasControl) return;

            MoveInput.Update();
            CameraInput.Update();
            MouseCameraInput.Update();
            if (_rigidbody == null) return;
            UpdateMovement();
            if (interactor != null && Input.GetButtonDown(InteractAction))
                interactor.Interact();
        }


        private float attackCount;

        private void LateUpdate()
        {
            if (!HasControl) return;
            if (Camera == null) return;
            UpdateCamera();
        }


        private void UpdateCamera()
        {
            if (Camera == null)
                SetCamera();
            if (!Input.GetMouseButton((int)MouseButton.Right))
            {
                if (!Cursor.visible && Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            else
            {
                if (Cursor.visible && Cursor.lockState != CursorLockMode.Locked)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            var input = CameraInput;
            if (MouseCameraInput.DirectionVector != Vector2.zero)
                input = MouseCameraInput;
            var cameraVector = input.DirectionVector;
            _yaw += cameraVector.x * cameraSpeed  * input.Speed * Time.deltaTime;
            _pitch -= cameraVector.y * cameraSpeed * input.Speed * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, 0, 60);
            if (_yaw < -180)
                _yaw += 360;
            if (_yaw > 180)
                _yaw -= 360;
            _cameraParent.rotation = Quaternion.Slerp(_cameraParent.rotation, Quaternion.Euler(_pitch, _yaw, 0), 0.5f);
        }

        private void UpdateMovement()
        {
            if (PhysicalObject != null)
            {
                var moveVector = MoveInput.ToMovementVector();
                var forward = _cameraTransform.forward;
                var right = _cameraTransform.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();
                var desiredMoveDirection = forward * moveVector.z + right * moveVector.x;
                _rigidbody.MovePosition(t.position + desiredMoveDirection * speed * Time.deltaTime);
                if (desiredMoveDirection != Vector3.zero)
                {
                    _container.ObjectHolder.rotation = Quaternion.LookRotation(desiredMoveDirection, Vector3.up);
                }
            }
        }
        
        private void Reset()
        {
            var system = GetComponent<PlayerControlSystem>();
            if (system.Controller == null)
            {
                system.Controller = this;
            }
        }
    }
}