using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Request Icon Database", fileName = "RequestIconDatabaseSO")]
    public class RequestIconDatabaseSO : ScriptableObject
    {
        [System.Serializable]
        public class CargoRequestIcon
        {
            public int CargoTypeId;
            public string RequestIconKey; // mapped to cargo addressables
        }
        
        public CargoRequestIcon[] CargoIcons; // mapped to cargoTypeId

        public string GetIconKeyForCargo(int cargoTypeId)
        {
            if (CargoIcons == null) return null;

            for (int i = 0; i < CargoIcons.Length; i++)
            {
                if (CargoIcons[i].CargoTypeId == cargoTypeId)
                {
                    return CargoIcons[i].RequestIconKey;
                }
            }

            return null;
        }
    }
}