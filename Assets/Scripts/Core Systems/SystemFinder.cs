using Game.Saving;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// a little system locator so other scripts can find systems without singletons everywhere
    /// </summary>
    public class SystemFinder : MonoBehaviour
    {
        public static SystemFinder Instance { get; private set; }

        [Header("Systems")]
        public Game.Saving.SaveSystem SaveSystem;
        public Game.Core.GameSession GameSession; // currently not in use but might use in project 2

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (SaveSystem == null) SaveSystem = FindFirstObjectByType<SaveSystem>();
        }
    }
}

