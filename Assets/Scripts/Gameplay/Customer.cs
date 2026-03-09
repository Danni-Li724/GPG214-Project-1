using System.Threading.Tasks;
using Game.Core;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class Customer : MonoBehaviour, IInteractable
    {
        [Header("Request")]
        public CustomerRequest Request { get; private set; }

        [Header("Reaction")]
        [SerializeField] private Transform reactionAnchor;
        [SerializeField] private SpriteRenderer reactionRenderer;

        private AddressablesAssetProvider assetProvider;
        private ReactionKeysSO reactionKeys;

        public void Init(CustomerRequest request, AddressablesAssetProvider provider, ReactionKeysSO keys)
        {
            Request = request;
            assetProvider = provider;
            reactionKeys = keys;

            if (reactionRenderer != null)
            {
                reactionRenderer.enabled = false;
            }
        }

        public bool CanInteract(PlayerInteract player)
        {
            return player != null && player.CarriedCargo != null;
        }

        public async void Interact(PlayerInteract player)
        {
            if (player == null || player.CarriedCargo == null) return;

            Cargo cargo = player.CarriedCargo;

            bool cargoMatch = cargo.Type != null && cargo.Type.CargoTypeId == Request.CargoTypeId;
            bool packagingMatch = cargo.Packaging.HasValue && cargo.Packaging.Value == Request.PackagingType;

            bool success = cargoMatch && packagingMatch;
            await ShowReactionAsync(success);
            player.DropAndDestroyCargo();
        }

        private async Task ShowReactionAsync(bool success)
        {
            if (reactionRenderer == null || reactionKeys == null || assetProvider == null) return;

            string key = success ? reactionKeys.SmileKey : reactionKeys.SadKey;
            Sprite sprite = await assetProvider.LoadAsync<Sprite>(key);

            if (sprite != null)
            {
                reactionRenderer.sprite = sprite;
                reactionRenderer.enabled = true;
            }
        }
    }
}
