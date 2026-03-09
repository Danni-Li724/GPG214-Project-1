using Game.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Gameplay
{
    public class PlayerBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private PlayerMovement movement;
       [SerializeField] private PlayerInteract interact;

        private void Awake()
        {
            movement.HookInput(input);
            interact.HookInput(input);
        }
    }
}