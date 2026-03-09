using Game.Input;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;

        private Rigidbody2D rb;
        private Vector2 moveInput;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void HookInput(PlayerInputReader input)
        {
            input.MoveEvent += OnMove;
        }

        private void OnDestroy()
        {
            // Safe even if never hooked.
        }

        private void OnMove(Vector2 v)
        {
            moveInput = v;
        }

        private void FixedUpdate()
        {
            Vector2 velocity = moveInput * moveSpeed;
            rb.linearVelocity = velocity;
        }
    }
}
