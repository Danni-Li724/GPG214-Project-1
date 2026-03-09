using System.Threading.Tasks;
using Game.Core;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class Cargo : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody2D rb;

        public CargoTypeSO Type { get; private set; }
        public PackagingType? Packaging { get; private set; }

        private AddressablesAssetProvider assetProvider;

        public async Task InitAsync(CargoTypeSO type, AddressablesAssetProvider provider)
        {
            Type = type;
            assetProvider = provider;

            // stream base sprite
            Sprite baseSprite = await assetProvider.LoadAsync<Sprite>(Type.BaseSpriteKey);
            if (baseSprite != null)
            {
                spriteRenderer.sprite = baseSprite;
            }
        }

        public async Task ApplyPackagingAsync(PackagingType packagingType)
        {
            Packaging = packagingType;

            int index = (int)packagingType;
            if (Type.PackagedSpriteKeys == null || index < 0 || index >= Type.PackagedSpriteKeys.Length)
            {
                Debug.LogWarning("PackagedSpriteKeys not set on CargoTypeSO");
                return;
            }

            string key = Type.PackagedSpriteKeys[index];
            Sprite packagedSprite = await assetProvider.LoadAsync<Sprite>(key);
            if (packagedSprite != null)
            {
                spriteRenderer.sprite = packagedSprite;
            }
        }

        public void SetCarried(bool isCarried, Transform carryParent)
        {
            if (isCarried)
            {
                transform.SetParent(carryParent, true);
                if (rb != null) rb.simulated = false;
            }
            else
            {
                transform.SetParent(null, true);
                if (rb != null) rb.simulated = true;
            }
        }
    }
}