using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actions;

        public event Action<Vector2> MoveEvent;
        public event Action InteractEvent;

        private InputAction move;
        private InputAction interact;

        private void Awake()
        {
            move = actions.FindAction("Move", true);
            interact = actions.FindAction("Interact", true);
        }

        private void OnEnable()
        {
            move.Enable();
            interact.Enable();

            move.performed += OnMove;
            move.canceled += OnMove;
            interact.performed += OnInteract;
        }

        private void OnDisable()
        {
            move.performed -= OnMove;
            move.canceled -= OnMove;
            interact.performed -= OnInteract;

            move.Disable();
            interact.Disable();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveEvent?.Invoke(ctx.ReadValue<Vector2>());
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            InteractEvent?.Invoke();
        }
    }
}