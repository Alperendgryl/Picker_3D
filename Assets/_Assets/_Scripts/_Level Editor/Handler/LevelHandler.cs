using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour, ILevelHandler
{
    public event Action OnLevelLoaded;

    private string selectedLevelFile;

    [SerializeField] private int platformCount;
    [SerializeField] public GameObject[] itemPrefabs;

    [Header("GameObjects")]
    [SerializeField] private GameObject platform;
    [SerializeField] public GameObject level;
    [SerializeField] private GameObject levelsButtonPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private TMP_Text currentLevelTXT;

    private void Start()
    {
        SetupLevel();
        GetLevelNames();
    }

    public void SaveLevel()
    {
        int nextLevelNumber = 1;
        while (File.Exists(Path.Combine(Application.persistentDataPath, "Level" + nextLevelNumber + ".json")))
        {
            nextLevelNumber++;
        }

        string fileName = "Level" + nextLevelNumber + ".json";
        SaveLevelToFile(fileName);

        // Update the selectedLevelFile and create a new button for it
        selectedLevelFile = "Level" + nextLevelNumber;
        GameObject newButton = Instantiate(levelsButtonPrefab, content);

        // Set the button text to the saved Level name
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = Path.GetFileNameWithoutExtension(Application.persistentDataPath + "/" + fileName);

        // Add a click listener to set the levelFile when the button is clicked
        Button savedLevels = newButton.GetComponent<Button>();
        savedLevels.onClick.AddListener(() => SetSelectedLevel(newButton));
    }

    public void UpdateLevel()
    {
        if (string.IsNullOrEmpty(selectedLevelFile))
        {
            Debug.LogWarning("No Level file selected to update");
            return;
        }

        string fileName = selectedLevelFile + ".json";
        SaveLevelToFile(fileName);
    }

    public void ResetLevel()
    {
        foreach (Transform child in level.transform)
        {
            Destroy(child.gameObject);
        }
        SetupLevel();
    }

    public void SetupLevel()
    {
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < platformCount; i++)
        {
            Instantiate(platform, spawnPosition, Quaternion.identity, level.transform);
            spawnPosition += new Vector3(0, 0, 10);
        }
    }

    public void GetLevelNames()
    {
        // Find all saved levels in the persistent data path
        string[] levelFiles = Directory.GetFiles(Application.persistentDataPath, "Level*.json");

        // Iterate through the found files and create a button for each
        foreach (string levelFile in levelFiles)
        {
            // Instantiate the button prefab as a child of the content GameObject
            GameObject newButton = Instantiate(levelsButtonPrefab, content);

            // Set the button text to the saved Level name
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = Path.GetFileNameWithoutExtension(levelFile);

            // Add a click listener to set the levelFile when the button is clicked
            Button savedLevels = newButton.GetComponent<Button>();
            savedLevels.onClick.AddListener(() => SetSelectedLevel(newButton));
        }
    }

    public void LoadLevel()
    {
        if (string.IsNullOrEmpty(selectedLevelFile))
        {
            Debug.LogWarning("No Level file selected");
            return;
        }

        // Read the JSON string from the selected Level file
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, selectedLevelFile + ".json"));

        // Deserialize the JSON string to a LevelData instance
        LevelData gameData = JsonUtility.FromJson<LevelData>(json);

        // Reset the Level
        ResetLevel();

        // Instantiate a new Level GameObject
        GameObject newLevel = new GameObject("Level");
        newLevel.tag = "Level";

        // Instantiate the saved Level design
        for (int i = 0; i < gameData.levelChildren.Count; i++)
        {
            LevelData.ObjectData objectData = gameData.levelChildren[i];
            Instantiate(itemPrefabs[objectData.prefabIndex], objectData.position, objectData.rotation, newLevel.transform);
        }

        // Delete the active Level GameObject and replace it with the new one
        Destroy(level);
        level = newLevel;
        // Invoke the event to notify subscribers that the Level has been loaded
        OnLevelLoaded?.Invoke();
    }

    private void SaveLevelToFile(string fileName)
    {
        // Create a new instance of the LevelData class
        LevelData gameData = new LevelData();

        // Get all the children of the "Level" GameObject and add them to the levelChildren list
        Transform levelTransform = level.transform;
        gameData.levelChildren = new List<LevelData.ObjectData>();
        foreach (Transform child in levelTransform)
        {
            int prefabIndex = Array.FindIndex(itemPrefabs, prefab => prefab.name == child.gameObject.name.Replace("(Clone)", "").Trim());
            if (prefabIndex >= 0)
            {
                LevelData.ObjectData objectData = new LevelData.ObjectData(prefabIndex, child.position, child.rotation);
                gameData.levelChildren.Add(objectData);
            }
            else
            {
                Debug.LogWarning("Prefab not found for: " + child.gameObject.name);
            }
        }

        // Serialize the LevelData instance to a JSON string
        string json = JsonUtility.ToJson(gameData);
        Debug.Log("Saved JSON: " + json);

        // Save the JSON string to a file with the provided file name
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }

    public void SetSelectedLevel(GameObject button)
    {
        selectedLevelFile = button.transform.GetChild(0).GetComponent<TMP_Text>().text.Replace("Level: ", "");
        currentLevelTXT.text = selectedLevelFile;
        Debug.Log("Active Level : " + selectedLevelFile);
        LoadLevel();
    }
}
