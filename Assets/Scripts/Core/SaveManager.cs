using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public string lastPlayedLevel = "Level_1_1";
    public bool[] unlockedLevels = new bool[30]; // 假设最多10关
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    private GameSaveData currentSaveData;
    private const string SAVE_KEY = "GameSaveData";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        string saveJson = PlayerPrefs.GetString(SAVE_KEY, "");
        if (!string.IsNullOrEmpty(saveJson))
        {
            currentSaveData = JsonUtility.FromJson<GameSaveData>(saveJson);
        }
        else
        {
            currentSaveData = new GameSaveData();
            currentSaveData.unlockedLevels[0] = true; // 第一关默认解锁
            SaveGame();
        }
    }

    public void SaveGame()
    {
        string saveJson = JsonUtility.ToJson(currentSaveData);
        PlayerPrefs.SetString(SAVE_KEY, saveJson);
        PlayerPrefs.Save();
    }

    public void UnlockLevel(string levelName)
    {
        // 简单实现：根据关卡名解锁
        int levelIndex = GetLevelIndex(levelName);
        if (levelIndex >= 0 && levelIndex < currentSaveData.unlockedLevels.Length)
        {
            currentSaveData.unlockedLevels[levelIndex] = true;
            SaveGame();
        }
    }

    public bool IsLevelUnlocked(string levelName)
    {
        int levelIndex = GetLevelIndex(levelName);
        return levelIndex >= 0 && levelIndex < currentSaveData.unlockedLevels.Length 
            && currentSaveData.unlockedLevels[levelIndex];
    }

    public void SetLastPlayedLevel(string levelName)
    {
        currentSaveData.lastPlayedLevel = levelName;
        SaveGame();
    }

    public string GetLastPlayedLevel()
    {
        return currentSaveData.lastPlayedLevel;
    }

    public void ResetProgress()
    {
        currentSaveData = new GameSaveData();
        currentSaveData.unlockedLevels[0] = true;
        SaveGame();
    }

    private int GetLevelIndex(string levelName)
    {
        // 简单解析关卡索引，如 "Level_1_1" -> 0, "Level_1_2" -> 1
        string[] parts = levelName.Split('_');
        if (parts.Length >= 3 && int.TryParse(parts[2], out int levelNum))
        {
            return levelNum - 1;
        }
        return -1;
    }
}