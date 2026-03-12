using System.Threading.Tasks;
using Game.Core;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class Customer : MonoBehaviour, IInteractable
    {
        [Header("Renderers")]
        [SerializeField] private SpriteRenderer bodyRenderer;
        [SerializeField] private SpriteRenderer requestRenderer;
        [SerializeField] private SpriteRenderer reactionRenderer;

        private AddressablesAssetProvider assetProvider;
        private ReactionKeysSO reactionKeys;
        private RequestIconDatabaseSO requestIcons;
        
        private ScoreManager scoreManager;

        public CustomerRequest Request { get; private set; }

        public async Task InitAsync(
            CustomerRequest request,
            string appearanceKey,
            AddressablesAssetProvider provider,
            ReactionKeysSO reactions,
            RequestIconDatabaseSO requestIconDb,
            ScoreManager scores = null
        )
        {
            Request = request;
            assetProvider = provider;
            reactionKeys = reactions;
            requestIcons = requestIconDb;
            scoreManager = scores;

            // hide reaction
            if (reactionRenderer != null) reactionRenderer.enabled = false;

            // stream appearance
            if (bodyRenderer != null && !string.IsNullOrWhiteSpace(appearanceKey))
            {
                Sprite appearance = await assetProvider.LoadAsync<Sprite>(appearanceKey);
                if (appearance != null) bodyRenderer.sprite = appearance;
            }

            // stream request (cargo icon)
            if (requestRenderer != null && requestIcons != null)
            {
                string requestKey = requestIcons.GetIconKeyForCargo(Request.CargoTypeId);
                Sprite requestSprite = await assetProvider.LoadAsync<Sprite>(requestKey);
                if (requestSprite != null)
                {
                    requestRenderer.sprite = requestSprite;
                    requestRenderer.enabled = true;
                }
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
            if (!cargoMatch)
            {
                scoreManager?.RecordWrongCargo();
            }
            else if (!packagingMatch)
            {
                scoreManager?.RecordRightCargoWrongPackaging();
            }
            else
            {
                scoreManager?.RecordPerfectDelivery();
            }
            bool success = cargoMatch && packagingMatch;
            Debug.Log(
                $"[DELIVER] Customer='{name}' wants CargoId={Request.CargoTypeId}, Pack={Request.PackagingType} | " +
                $"PlayerCargo='{cargo.name}' CargoId={(cargo.Type != null ? cargo.Type.CargoTypeId : -1)}, Pack={(cargo.Packaging.HasValue ? cargo.Packaging.Value.ToString() : "NULL")}"
            );
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