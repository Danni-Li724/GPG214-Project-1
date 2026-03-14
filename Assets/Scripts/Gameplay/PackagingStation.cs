using System.Threading.Tasks;
using UnityEngine;
using Game.Data;
using Game.Core;

namespace Game.Gameplay
{
    public class PackagingStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private CargoQueue cargoQueue;
        [SerializeField] private PackagingOptionsPanel panel;
        
        [Header("Packaging Sfx")]
        [SerializeField] private PackagingSfxDatabaseSO packagingSfxDatabase;
        [SerializeField] private AudioLoader audioLoader;
        [SerializeField] private SfxPlayer sfxPlayer;

        private void Awake()
        {
            if (panel != null) panel.Hide();
        }

        public bool CanInteract(PlayerInteract player)
        {
            return cargoQueue != null && cargoQueue.HasCargoWaitingForPackaging();
        }

        public void Interact(PlayerInteract player)
        {
            if (panel == null) return;
            panel.Show(this);
        }

        public async Task SelectPackagingAsync(PackagingType type)
        {
            if (cargoQueue == null) return;
            Cargo cargo = cargoQueue.GetActiveCargoForPackaging();
            if (cargo == null) return;
            CargoMover mover = cargo.GetComponent<CargoMover>();
            if (mover == null || mover.CurrentState != CargoMover.State.WaitingForPackaging) return;
            await cargo.ApplyPackagingAsync(type);
            if (packagingSfxDatabase != null && audioLoader != null && sfxPlayer != null)
            {
                if (packagingSfxDatabase.TryGet(type, out var entry))
                {
                    audioLoader.LoadWav(entry.WavFilename + ".wav", clip =>
                    {
                        sfxPlayer.PlayOneShot(clip, 1f, 1f);
                    });
                }
            }
            mover.MarkPackagedAndContinue();
            cargoQueue.MarkActiveCargoPackaged();
            if (panel != null) panel.Hide();
        }
    }
}