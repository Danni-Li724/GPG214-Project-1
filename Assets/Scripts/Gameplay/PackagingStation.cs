using System.Threading.Tasks;
using Game.Core;
using UnityEngine;

namespace Game.Gameplay
{
    public class PackagingStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private PackagingOptionsPanel panel;

        private CargoQueue queue;

        public void Init(CargoQueue cargoQueue)
        {
            queue = cargoQueue;
            panel.Hide();
        }

        public bool CanInteract(PlayerInteract player)
        {
            // player just needs to be near station; cargo must be waiting at packaging
            return queue != null && queue.HasCargoWaitingForPackaging();
        }

        public void Interact(PlayerInteract player)
        {
            panel.Show(this);
        }

        public async Task SelectPackagingAsync(PackagingType type)
        {
            if (queue == null) return;

            Cargo cargo = queue.PeekWaitingCargo();
            if (cargo == null) return;

            await cargo.ApplyPackagingAsync(type);
            queue.MarkFrontCargoPackaged();
            panel.Hide();
        }
    }
}