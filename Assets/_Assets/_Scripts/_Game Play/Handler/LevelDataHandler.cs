using System.IO;
using UnityEngine;

public class LevelDataHandler : MonoBehaviour
{
    public static LevelDataHandler Instance { get; private set; }

    public int playerLevel;
    public int levelArrayIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGameData()
    {
        GameLevelData data = new GameLevelData();
        data.playerLevel = playerLevel;
        data.levelArrayIndex = levelArrayIndex;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }

    public void LoadGameData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameLevelData data = JsonUtility.FromJson<GameLevelData>(json);

            playerLevel = data.playerLevel;
            levelArrayIndex = data.levelArrayIndex;
        }
        else
        {
            // Handle error - there is no saved game data
        }
    }
}
