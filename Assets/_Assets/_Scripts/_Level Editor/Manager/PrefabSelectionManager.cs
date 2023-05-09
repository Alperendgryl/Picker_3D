using System;
using UnityEngine;

public class PrefabSelectionManager : MonoBehaviour
{
    private LevelHandler levelHandler;
    private IInputHandler inputHandler;
    private GameObject prefabToInstantiate;

    public GameObject level
    {
        get { return inputHandler.level; }
        set { inputHandler.level = value; }
    }
    public IInputHandler InputHandler
    {
        get { return inputHandler; }
        set { inputHandler = value; }
    }
    private void Start()
    {
        levelHandler = FindObjectOfType<LevelHandler>();
        if (levelHandler != null) levelHandler.OnLevelLoaded += UpdatePrefabToInstantiate;

        inputHandler = FindObjectOfType<InputHandler>();
        if (inputHandler != null) inputHandler.level = levelHandler.level;
    }
    private void OnDestroy()
    {
        levelHandler.OnLevelLoaded -= UpdatePrefabToInstantiate;
    }

    public void SetPrefabToInstantiate(int index)
    {
        prefabToInstantiate = levelHandler.itemPrefabs[index];
        Debug.Log("SetPrefabToInstantiate : " + prefabToInstantiate.name);
    }

    private void UpdatePrefabToInstantiate()
    {
        if (prefabToInstantiate != null)
        {
            int prefabIndex = Array.FindIndex(levelHandler.itemPrefabs, prefab => prefab.name == prefabToInstantiate.name);

            if (prefabIndex >= 0) prefabToInstantiate = levelHandler.itemPrefabs[prefabIndex];
        }
        level = levelHandler.level;
        Debug.Log("Update Prefab To Instantiate");
    }

    public GameObject GetPrefabToInstantiate()
    {
        return prefabToInstantiate;
    }

    public void SetPrefabAsNull()
    {
        prefabToInstantiate = null;
    }
}
