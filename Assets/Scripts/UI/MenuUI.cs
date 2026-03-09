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

        private void Awake()
        {
            if (startButton != null)
                startButton.onClick.AddListener(OnStartClicked);
        }

        private void Start()
        {
            var save = SystemFinder.Instance.SaveSystem;
            var profile = save.LoadProfileOrDefault();

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

            profile.PlayerName = (nameField != null) ? nameField.text : "Player";

            int parsedAge = 18;
            if (ageField != null)
            {
                int.TryParse(ageField.text, out parsedAge);
            }
            profile.PlayerAge = parsedAge;

            profile.IconId = (iconSelector != null) ? iconSelector.SelectedIconId : 0;
            profile.FavColorId = (favColourDropdown != null) ? favColourDropdown.value : 0;

            return profile;
        }
    }
}
