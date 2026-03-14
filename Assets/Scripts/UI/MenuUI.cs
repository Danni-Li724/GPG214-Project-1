using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;
using Game.Saving;

namespace Game.UI
{
    public class MenuUIController : MonoBehaviour
    {
       [Header("Inputs")]
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_InputField ageField;

        [Header("Icon")]
        [SerializeField] private MenuIconSelector iconSelector;

        [Header("Fav Colour")]
        [SerializeField] private TMP_Dropdown favColourDropdown;

        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private string defaultProfileJsonPath = "profile_default.json";
        private PlayerProfileData defaultProfileFromJson;

        private void Awake()
        {
            if (startButton != null)
                startButton.onClick.AddListener(OnStartClicked);
        }

        private void Start()
        {
            StartCoroutine(LoadAndApplyProfile());
        }
        private IEnumerator LoadAndApplyProfile()
        {
            var save = SystemFinder.Instance.SaveSystem;
            if (save.HasProfile())
            {
                var profile = save.LoadProfileOrDefault();
                defaultProfileFromJson = profile;
                ApplyProfileToUI(profile);
                yield break;
            }

            // if no input/saved profile, try to load default from StreamingAssets
            string json = null;
            yield return StreamingAssets.LoadText(defaultProfileJsonPath, t => json = t);

            if (!string.IsNullOrWhiteSpace(json))
            {
                PlayerProfileData profile = JsonUtility.FromJson<PlayerProfileData>(json);
                if (profile != null)
                {
                    defaultProfileFromJson = profile;
                    ApplyProfileToUI(profile);
                    yield break;
                }
            }

            // fallback 
            ApplyProfileToUI(save.LoadProfileOrDefault());
        }

        private void ApplyProfileToUI(PlayerProfileData profile)
        {
            if (profile == null) return;

            if (nameField != null) nameField.text = profile.PlayerName;
            if (ageField != null) ageField.text = profile.PlayerAge.ToString();
            if (iconSelector != null) iconSelector.SetSelectedIcon(profile.IconId);
            if (favColourDropdown != null) favColourDropdown.value = profile.FavColorId;
        }

        private void OnStartClicked()
        {
            var profile = BuildProfileFromUI();

            SystemFinder.Instance.SaveSystem.SaveProfile(profile);
            SystemFinder.Instance.GameSession.ResetSession();

            SceneLoader.LoadGameplay();
        }

        private PlayerProfileData BuildProfileFromUI()
        {
            var profile = new PlayerProfileData();
            string fallbackName = (defaultProfileFromJson != null) ? defaultProfileFromJson.PlayerName : "Player";
            int fallbackAge = (defaultProfileFromJson != null) ? defaultProfileFromJson.PlayerAge : 18;
            int fallbackIconId = (defaultProfileFromJson != null) ? defaultProfileFromJson.IconId : 0;
            int fallbackColorId = (defaultProfileFromJson != null) ? defaultProfileFromJson.FavColorId : 0;

            profile.PlayerName = (nameField != null && !string.IsNullOrWhiteSpace(nameField.text))
                ? nameField.text
                : fallbackName;

            int parsedAge = fallbackAge;
            if (ageField != null)
            {
                if (!int.TryParse(ageField.text, out parsedAge))
                {
                    parsedAge = fallbackAge;
                }
            }
            // profile.PlayerName = (nameField != null && !string.IsNullOrWhiteSpace(nameField.text))
            //     ? nameField.text
            //     : "Player";
            //
            // int parsedAge = 18;
            // if (ageField != null)
            // {
            //     if (!int.TryParse(ageField.text, out parsedAge))
            //     {
            //         parsedAge = 18;
            //     }
            // }
            profile.PlayerAge = parsedAge;
            profile.IconId = (iconSelector != null) ? iconSelector.SelectedIconId : fallbackIconId;
            profile.FavColorId = (favColourDropdown != null) ? favColourDropdown.value : fallbackColorId;
            return profile;
        }
    }
}
