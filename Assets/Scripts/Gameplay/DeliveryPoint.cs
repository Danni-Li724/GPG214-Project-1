using UnityEngine;

namespace Game.Gameplay
{
    public class DeliveryPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private CargoQueue queue;

        public bool CanInteract(PlayerInteract player)
        {
            if (player == null) return false;
            if (player.CarriedCargo != null) return false;
            return queue != null && queue.HasCargoWaitingAtDelivery();
        }

        public void Interact(PlayerInteract player)
        {
            Cargo cargo = queue.TakeCargoReadyForPickup();
            if (cargo == null) return;

            var mover = cargo.GetComponent<CargoMover>();
            if (mover != null) mover.MarkPickedUp();

            player.PickUpCargo(cargo);
        }
    }
}
