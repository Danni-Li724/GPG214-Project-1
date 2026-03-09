using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{

    [SerializeField] private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private int healthIncrementAmount;
    [SerializeField] private float progressIncrementAmount;
    [SerializeField] private int coinsIncrementAmount;

    private void Start()
    {
        Refresh();
    }

    public void HealthPlus()
    {
        if (playerData == null) return;

        playerData.AddHealth(healthIncrementAmount);
        Refresh();
    }

    public void HealthMinus()
    {
        if (playerData == null) return;

        playerData.AddHealth(-healthIncrementAmount);
        Refresh();
    }

    public void ProgressPlus()
    {
        if (playerData == null) return;

        playerData.AddProgress(progressIncrementAmount);
        Refresh();
    }

    public void ProgressMinus()
    {
        if (playerData == null) return;

        playerData.AddProgress(-progressIncrementAmount);
        Refresh();
    }

    public void CoinsPlus()
    {
        if (playerData == null) return;

        playerData.AddCoins(coinsIncrementAmount);
        Refresh();
    }

    public void CoinsMinus()
    {
        if (playerData == null) return;

        playerData.AddCoins(-coinsIncrementAmount);
        Refresh();
    }

    public void SaveJson()
    {
        if (playerData == null) return;

        playerData.SaveToJson();
        Refresh();
    }

    public void LoadJson()
    {
        if (playerData == null) return;

        bool loaded = playerData.LoadFromJson();
        if (loaded)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        if (playerData == null) return;

        if (healthText != null) healthText.text = "Health: " + playerData.Health.ToString();
        if (progressText != null) progressText.text = "Progress: " + playerData.Progress.ToString("0.##") + " %";
        if (coinsText != null) coinsText.text = "Coins: " + playerData.Coins.ToString();
    }
}
