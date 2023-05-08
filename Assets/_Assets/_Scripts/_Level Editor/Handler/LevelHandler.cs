using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour, ILevelController
{
    [SerializeField] private int platformCount;
    [SerializeField] public GameObject[] itemPrefabs;

    [Header("GameObjects")]
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform content;

    private string selectedLevelFile;
    private void Start()
    {
        SetupLevel();
        GetLevelNames();
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
            GameObject newButton = Instantiate(buttonPrefab, content);

            // Set the button text to the saved level name
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = Path.GetFileNameWithoutExtension(levelFile);

            // Add a click listener to set the levelFile when the button is clicked
            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SetSelectedLevel(newButton));
        }
    }

    public void LoadLevel()
    {
        if (string.IsNullOrEmpty(selectedLevelFile))
        {
            Debug.LogWarning("No level file selected");
            return;
        }

        // Read the JSON string from the selected level file
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, selectedLevelFile + ".json"));

        // Deserialize the JSON string to a GameData instance
        GameData gameData = JsonUtility.FromJson<GameData>(json);

        // Reset the level
        ResetLevel();

        // Delete the active level GameObject
        Destroy(level);

        // Instantiate a new level GameObject
        level = new GameObject("Level");

        // Instantiate the saved level design
        for (int i = 0; i < gameData.levelDesignChildren.Count; i++)
        {
            GameData.ObjectData objectData = gameData.levelDesignChildren[i];
            Instantiate(itemPrefabs[objectData.prefabIndex], objectData.position, objectData.rotation, level.transform);
        }
    }


    private int savedLevelCount = 0;
    public void SaveLevel()
    {
        // Increment the savedLevelCount by one
        savedLevelCount++;

        // Create a new instance of the GameData class
        GameData gameData = new GameData();

        // Set the levelDesign field to the "level" GameObject
        gameData.levelDesign = GameObject.FindGameObjectWithTag("Level");

        // Get all the children of the "level" GameObject and add them to the levelDesignChildren list
        Transform levelTransform = gameData.levelDesign.transform;
        gameData.levelDesignChildren = new List<GameData.ObjectData>();
        foreach (Transform child in levelTransform)
        {
            int prefabIndex = Array.FindIndex(itemPrefabs, prefab => prefab.name == child.gameObject.name);
            if (prefabIndex >= 0)
            {
                GameData.ObjectData objectData = new GameData.ObjectData
                {
                    prefabIndex = prefabIndex,
                    position = child.position,
                    rotation = child.rotation
                };
                gameData.levelDesignChildren.Add(objectData);
            }
        }

        // Serialize the GameData instance to a JSON string
        string json = JsonUtility.ToJson(gameData);

        // Save the JSON string to a file with the incremented level count in the file name
        string fileName = "Level" + savedLevelCount + ".json";
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);

        // Instantiate the button prefab as a child of the content GameObject
        GameObject newButton = Instantiate(buttonPrefab, content);

        // Set the button text to the saved level name
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = Path.GetFileNameWithoutExtension(Application.persistentDataPath + "/" + fileName);
    }

    public void ResetLevel()
    {
        foreach (Transform child in level.transform)
        {
            if (child.CompareTag("Platform")) continue;
            Destroy(child.gameObject);
        }
    }

    public void SetSelectedLevel(GameObject button)
    {
        selectedLevelFile = button.transform.GetChild(0).GetComponent<TMP_Text>().text.Replace("Level: ", "");
        Debug.Log("Selected File Name : " + selectedLevelFile);
    }

}
