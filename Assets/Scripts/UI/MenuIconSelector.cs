using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MenuIconSelector : MonoBehaviour
    {
        [System.Serializable]
        public class IconButton
        {
            public int IconId;
            public Button Button;
        }

        [SerializeField] private IconButton[] iconButtons;

        public int SelectedIconId { get; private set; }

        private void Awake()
        {
            HookButtons();
        }

        private void HookButtons()
        {
            if (iconButtons == null) return;

            for (int i = 0; i < iconButtons.Length; i++)
            {
                int index = i; 
                if (iconButtons[index] != null && iconButtons[index].Button != null)
                {
                    iconButtons[index].Button.onClick.AddListener(OnIconButtonClicked);
                }
            }
        }
        private void OnIconButtonClicked()
        {
            // find which button invoked this
            var clicked = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (clicked == null) return;

            for (int i = 0; i < iconButtons.Length; i++)
            {
                if (iconButtons[i] != null && iconButtons[i].Button != null)
                {
                    if (iconButtons[i].Button.gameObject == clicked)
                    {
                        SelectedIconId = iconButtons[i].IconId;
                        return;
                    }
                }
            }
        }
        public void SetSelectedIcon(int iconId)
        {
            SelectedIconId = iconId;
        }
    }
}