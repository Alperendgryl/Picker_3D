using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Expose an event to notify when a level is selected
    public static event Action<int> OnLevelSelected;

    public List<LevelData> GetAllLevels()
    {
        List<LevelData> levels = new List<LevelData>();
        string[] levelFiles = Directory.GetFiles(Application.dataPath, "LevelData_*.json");

        foreach (string levelFile in levelFiles)
        {
            string json = File.ReadAllText(levelFile);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);
            levels.Add(levelData);
        }

        return levels;
    }

    public void SelectLevel(int levelNumber)
    {
        OnLevelSelected?.Invoke(levelNumber);
    }

    public int GetNumberOfLevels()
    {
        return GetAllLevels().Count;
    }

}
