using UnityEngine;
using UnityEngine.UI;
using Game.Gameplay;

namespace Game.UI
{
    public class TestCargoButtonPanel : MonoBehaviour
    {
        [System.Serializable]
        public class CargoButtonEntry
        {
            public int CargoTypeId;
            public Button Button;
        }

        [SerializeField] private TestCargoSpawner spawner;
        [SerializeField] private CargoButtonEntry[] buttons;
        
        [SerializeField] private Button randomButton;
        [SerializeField] private Button clearButton;

        private void Awake()
        {
            HookButtons();
        }

        private void HookButtons()
        {
            if (buttons != null)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] != null && buttons[i].Button != null)
                    {
                        buttons[i].Button.onClick.AddListener(OnAnyCargoButtonClicked);
                    }
                }
            }

            if (randomButton != null) randomButton.onClick.AddListener(OnRandomClicked);
            if (clearButton != null) clearButton.onClick.AddListener(OnClearClicked);
        }

        private void OnAnyCargoButtonClicked()
        {
            if (spawner == null) return;

            var selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (selected == null) return;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null && buttons[i].Button != null)
                {
                    if (buttons[i].Button.gameObject == selected)
                    {
                        spawner.SpawnCargoById(buttons[i].CargoTypeId);
                        return;
                    }
                }
            }
        }

        private void OnRandomClicked()
        {
            if (spawner == null) return;
            spawner.SpawnRandomCargo();
        }
        private void OnClearClicked()
        {
            if (spawner == null) return;
            spawner.ClearSpawned();
        }
    }
}
