using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Customer Appearance Database", fileName = "CustomerAppearanceDatabaseSO")]
    public class CustomerAppearanceDatabaseSO : ScriptableObject
    {
        public string[] CustomerSpriteKeys;

        public string GetRandomKey(System.Random rng)
        {
            if (CustomerSpriteKeys == null || CustomerSpriteKeys.Length == 0) return null;
            return CustomerSpriteKeys[rng.Next(0, CustomerSpriteKeys.Length)];
        }
    }
}