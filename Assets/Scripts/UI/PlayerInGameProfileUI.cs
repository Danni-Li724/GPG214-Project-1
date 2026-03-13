using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game.Core;
using Game.Saving;

namespace Game.UI
{
    public class PlayerInGameProfileUI : MonoBehaviour
    {
         [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text ageText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image panelBackground;
        [SerializeField] private Color[] favColourPalette;
        [SerializeField] private float pixelsPerUnit = 100f;
        [SerializeField] private string iconPathPattern = "PlayerIcons/icon_{0}.png";

        private void Start()
        {
            // reads from what menu saved
            PlayerProfileData profile = SystemFinder.Instance.SaveSystem.LoadProfileOrDefault();
            ApplyTextAndColor(profile);
            StartCoroutine(LoadAndApplyIcon(profile));
        }
        private void ApplyTextAndColor(PlayerProfileData profile)
        {
            if (profile == null) return;

            if (nameText != null) nameText.text = "Name: " + profile.PlayerName;
            if (ageText != null) ageText.text = "Age: " + profile.PlayerAge.ToString();

            if (panelBackground != null && favColourPalette != null && favColourPalette.Length > 0)
            {
                int id = profile.FavColorId;
                if (id < 0 || id >= favColourPalette.Length) id = 0;
                panelBackground.color = favColourPalette[id];
            }
        }

        private System.Collections.IEnumerator LoadAndApplyIcon(PlayerProfileData profile)
        {
            if (profile == null || iconImage == null) yield break;

            int id = profile.IconId;
            if (id < 0) id = 0;

            string path = string.Format(iconPathPattern, id);

            Sprite loaded = null;
            yield return StreamingAssets.LoadSprite(path, pixelsPerUnit, s => loaded = s);

            if (loaded != null)
            {
                iconImage.sprite = loaded;
            }
        }
    }
}