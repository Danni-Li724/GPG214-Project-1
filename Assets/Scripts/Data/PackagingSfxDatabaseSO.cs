using UnityEngine;
using Game.Gameplay;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Audio/Packaging SFX Database", fileName = "PackagingSfxDatabaseSO")]
    public class PackagingSfxDatabaseSO : ScriptableObject
    {
        [System.Serializable]
        public class Entry
        {
            public PackagingType PackagingType;
            public string WavFilename; 
        }

        public Entry[] Entries;

        public bool TryGet(PackagingType type, out Entry entry)
        {
            entry = null;
            if (Entries == null) return false;

            for (int i = 0; i < Entries.Length; i++)
            {
                if (Entries[i].PackagingType == type)
                {
                    entry = Entries[i];
                    return true;
                }
            }
            return false;
        }
    }
}