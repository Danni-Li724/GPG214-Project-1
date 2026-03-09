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

        [Header("Icon Sprites")]
        [SerializeField] private Sprite[] iconSprites;

        private void Start()
        {
            // reads from what menu saved
            PlayerProfileData profile = SystemFinder.Instance.SaveSystem.LoadProfileOrDefault();
            Apply(profile);
        }

        private void Apply(PlayerProfileData profile)
        {
            if (profile == null) return;

            if (nameText != null) nameText.text = profile.PlayerName;
            if (ageText != null) ageText.text = profile.PlayerAge.ToString();

            if (iconImage != null && iconSprites != null)
            {
                int id = profile.IconId;
                if (id >= 0 && id < iconSprites.Length)
                {
                    iconImage.sprite = iconSprites[id];
                }
            }
        }
    }
}