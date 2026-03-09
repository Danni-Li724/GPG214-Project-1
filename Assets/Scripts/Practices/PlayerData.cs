using System;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [System.Serializable]
    private class PlayerSaveData
    {
        public int health;
        public float progress;
        public int coins;
    }
    [Header("Current")]
    [SerializeField] private int health = 60;
    [SerializeField] private float progress = 20.5f;
    [SerializeField] private int coins = 10;
    
    private string saveFilePath;
    private string savesFolderPath;

    public int Health => health;
    public float Progress => progress;
    public int Coins => coins;

    private void Start()
    {
        // string savesFolderPath = Path.Combine(Application.persistentDataPath, "Saves");
        // Directory.CreateDirectory(savesFolderPath);
        savesFolderPath = Path.Combine(Application.persistentDataPath, "Saves");
        Directory.CreateDirectory(savesFolderPath);
        // build full file
        saveFilePath = Path.Combine(savesFolderPath, "save1.json");
        Debug.Log("savesFolderPath: " + savesFolderPath);
        Debug.Log("saveFilePath: " + saveFilePath);
    }
    
    public void AddHealth(int amount)
    {
        health += amount;
    }

    public void AddProgress(float amount)
    {
        progress += amount;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }
    
    public void SaveToJson()
    {
        PlayerSaveData dataToSave = new PlayerSaveData();
        dataToSave.health = health;
        dataToSave.progress = progress;
        dataToSave.coins = coins;

        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("saved file to: " + saveFilePath);
        Debug.Log("saved JSON:\n" + json);
    }

    public bool LoadFromJson()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("file doesn't exist at" + saveFilePath);
            return false;
        }
        string loadedJson = File.ReadAllText(saveFilePath);
        PlayerSaveData loadedData = JsonUtility.FromJson<PlayerSaveData>(loadedJson);
        health = loadedData.health;
        progress = loadedData.progress;
        coins = loadedData.coins;
        Debug.Log("Loaded from: " + saveFilePath);
        Debug.Log("Loaded health: " + health);
        Debug.Log("Loaded progress: " + progress);
        Debug.Log("Loaded coins: " + coins);
        return true;
    }

    
    /// <summary>
    /// Old practice
    /// </summary>
    private void CreatePlayerSaveFile()
    {
        string savesFolderPath = Path.Combine(Application.persistentDataPath, "Saves");
        Directory.CreateDirectory(savesFolderPath);
        // build full file
        string saveFilePath = Path.Combine(savesFolderPath, "save1.json");
        
        // write json
        PlayerSaveData dataToSave = new PlayerSaveData();
        dataToSave.health = 60;
        dataToSave.progress = 20.5f;
        dataToSave.coins = 10;

        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Saved file to: " + saveFilePath);
        Debug.Log("Saved JSON:\n" + json);

        // read json
        if (File.Exists(saveFilePath))
        {
            string loadedJson = File.ReadAllText(saveFilePath);
            PlayerSaveData loadedData = JsonUtility.FromJson<PlayerSaveData>(loadedJson);

            Debug.Log("Loaded health: " + loadedData.health);
            Debug.Log("Loaded coins: " + loadedData.coins);
            Debug.Log("Loaded progress: " + loadedData.progress);
        }

    }
}
