using UnityEngine;
using UnityEngine.UI;
using Game.Core;

namespace Game.Gameplay
{
    public class ResultsManager : MonoBehaviour
    {
        [SerializeField] private Button seeResultsButton;
        [SerializeField] private ScoreManager scoreManager;
        private int totalCustomers;
        private int servedCustomers;

        private void Awake()
        {
            if (seeResultsButton != null)
            {
                seeResultsButton.gameObject.SetActive(false);
                seeResultsButton.onClick.AddListener(OnSeeResultsClicked);
            }
        }

        public void SetTotalCustomers(int total)
        {
            totalCustomers = Mathf.Max(0, total);
            servedCustomers = 0;

            if (seeResultsButton != null)
            {
                seeResultsButton.gameObject.SetActive(totalCustomers == 0);
            }
        }

        public void NotifyCustomerServed()
        {
            servedCustomers++;

            if (servedCustomers >= totalCustomers)
            {
                if (seeResultsButton != null)
                {
                    seeResultsButton.gameObject.SetActive(true);
                }
            }
        }

        private void OnSeeResultsClicked()
        {
            if (scoreManager != null)
            {
                scoreManager.SaveScoresToXml();
            }

            SceneLoader.LoadResults();
        }
    }
}