using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game.Core;
using Game.Saving;
using UnityEngine.Serialization;

namespace Game.UI
{
    public class ResultsUI : MonoBehaviour
    {
        [Header("Profile UI")] [SerializeField]
        private TMP_Text playerNameText;

        [SerializeField] private TMP_Text playerAgeText;
        [SerializeField] private Image playerIconImage;
        [SerializeField] private float pixelsPerUnit = 100f;
        [SerializeField] private string iconPath = "PlayerIcons/icon_{0}.png";

        [Header("Scores UI")] [SerializeField] private TMP_Text wrongCargoText;
        [SerializeField] private TMP_Text rightCargoWrongPackText;
        [SerializeField] private TMP_Text perfectText;

        private void Start()
        {
            // find player's profile from JSON 
            PlayerProfileData profile = SystemFinder.Instance.SaveSystem.LoadProfileOrDefault();
            ApplyProfile(profile);

            // find player's scores from XML
            var scoreData = MyXmlSerializer.LoadOrDefault(Game.Gameplay.ScorePaths.ScoreXmlPath);
            ApplyScores(scoreData);

            // stream player's chosen icon from StreamingAssets
            StartCoroutine(LoadAndApplyIcon(profile));
        }

        private void ApplyProfile(PlayerProfileData profile)
        {
            if (profile == null) return;

            if (playerNameText != null) playerNameText.text = profile.PlayerName;
            if (playerAgeText != null) playerAgeText.text = profile.PlayerAge.ToString();
        }

        private void ApplyScores(Game.Gameplay.ScoreResultsData data)
        {
            if (wrongCargoText != null) wrongCargoText.text = $"Wrong Cargo: {data.WrongCargoCount}";
            if (rightCargoWrongPackText != null)
                rightCargoWrongPackText.text = $"Wrong Packaging: {data.RightCargoWrongPackagingCount}";
            if (perfectText != null) perfectText.text = $"Perfect!: {data.PerfectDeliveryCount}";
        }

        private IEnumerator LoadAndApplyIcon(PlayerProfileData profile)
        {
            if (profile == null || playerIconImage == null) yield break;

            string path = string.Format(iconPath, profile.IconId);

            Sprite loaded = null;
            yield return Game.Core.StreamingAssets.LoadSprite(path, pixelsPerUnit, s => loaded = s);

            if (loaded != null)
            {
                playerIconImage.sprite = loaded;
            }
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}