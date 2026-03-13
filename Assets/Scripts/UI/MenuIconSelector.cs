using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;

namespace Game.UI
{
    public class MenuIconSelector : MonoBehaviour
    {
        [System.Serializable]
        public class IconButton
        {
            public int IconId;
            public Button Button;
            
            public Image TargetImage;
            public string IconRelativePathOverride;
        }

        [SerializeField] private IconButton[] iconButtons;
        [SerializeField] private float pixelsPerUnit = 100f; 

        public int SelectedIconId { get; private set; }

        private void Awake()
        {
            HookButtons();
            StartCoroutine(LoadAllIcons());
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
        
        private IEnumerator LoadAllIcons()
        {
            if (iconButtons == null) yield break;

            for (int i = 0; i < iconButtons.Length; i++)
            {
                IconButton entry = iconButtons[i];
                if (entry == null) continue;
                if (entry.TargetImage == null) continue;

                string path = entry.IconRelativePathOverride;
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = $"PlayerIcons/icon_{entry.IconId}.png";
                }

                Sprite loaded = null;
                yield return StreamingAssets.LoadSprite(path, pixelsPerUnit, s => loaded = s);

                if (loaded != null)
                {
                    entry.TargetImage.sprite = loaded;
                }
            }
        }
    }
}