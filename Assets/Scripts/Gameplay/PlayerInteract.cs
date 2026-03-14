using Game.Input;
using UnityEngine;

namespace Game.Gameplay
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Carry")]
        [SerializeField] private Transform carryPoint;

        public Cargo CarriedCargo { get; private set; }

        private IInteractable currentInteractable;

        public void HookInput(PlayerInputReader input)
        {
            input.InteractEvent += OnInteract;
        }

        private void OnInteract()
        {
            if (currentInteractable == null) return;
            if (!currentInteractable.CanInteract(this)) return;

            currentInteractable.Interact(this);
        }

        public void PickUpCargo(Cargo cargo)
        {
            CarriedCargo = cargo;
            cargo.SetCarried(true, carryPoint);
        }

        public void DropAndDestroyCargo()
        {
            if (CarriedCargo == null) return;

            CarriedCargo.SetCarried(false, null);
            Destroy(CarriedCargo.gameObject);
            CarriedCargo = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null && currentInteractable == interactable)
            {
                currentInteractable = null;
            }
        }
    }
}