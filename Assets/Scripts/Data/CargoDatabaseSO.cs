using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Cargo/CargoDatabase", fileName = "CargoDatabase")]
    public class CargoDatabaseSO : ScriptableObject
    {
        public CargoTypeSO[] CargoTypes;

        public CargoTypeSO GetById(int id)
        {
            if (CargoTypes == null) return null;

            for (int i = 0; i < CargoTypes.Length; i++)
            {
                if (CargoTypes[i] != null && CargoTypes[i].CargoTypeId == id)
                    return CargoTypes[i];
            }

            return null;
        }

        public CargoTypeSO GetRandom()
        {
            if (CargoTypes == null || CargoTypes.Length == 0) return null;

            int idx = Random.Range(0, CargoTypes.Length);
            return CargoTypes[idx];
        }
    }
}