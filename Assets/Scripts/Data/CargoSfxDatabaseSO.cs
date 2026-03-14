using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Audio/Cargo SFX Database", fileName = "CargoSfxDatabaseSO")]
    public class CargoSfxDatabaseSO : ScriptableObject
    {
        [System.Serializable]
        public class Entry
        {
            public int CargoTypeId;
            public string WavFilename; 
        }

        public Entry[] Entries;

        public string GetFilename(int cargoTypeId)
        {
            if (Entries == null) return null;

            for (int i = 0; i < Entries.Length; i++)
            {
                if (Entries[i].CargoTypeId == cargoTypeId)
                {
                    return Entries[i].WavFilename;
                }
            }

            return null;
        }
    }
}