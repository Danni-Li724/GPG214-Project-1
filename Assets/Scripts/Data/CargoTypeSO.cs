using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Cargo Type", fileName = "CargoTypeSO")]
    public class CargoTypeSO : ScriptableObject
    {
        [Header("Identity")]
        public int CargoTypeId;
        public string DisplayName;
        [Header("Prefab")]
        public GameObject CargoPrefab;
        [Header("Addressables Sprite Keys")]
        public string BaseSpriteKey; // Sprite key for cargo when it spawns (unpacked)
        public string[] PackagedSpriteKeys = new string[4]; // 4 sprite keys for the 4 packaging outcomes
        
        public Gameplay.PackagingType RequiredPackaging;
    }
}