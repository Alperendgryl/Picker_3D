using System.Collections.Generic;
using UnityEngine;

public class InfPlatform : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject poolPrefab;
    [SerializeField] private int poolSize = 25;
    [SerializeField] private Transform player;

    // Private fields
    private GameObject currentLevel;
    private GameObject infiniteTrigger;
    private int lastZPos = 0;
    private int platformCount = 0;

    private List<GameObject> platformPool;
    private List<GameObject> poolPool;

    // Initialization of platform and pool lists
    private void Awake()
    {
        platformPool = new List<GameObject>();
        poolPool = new List<GameObject>();
    }

    // Initial setup of level and triggers, creation of object pools
    private void Start()
    {
        SetupLevelAndTrigger();
        CreateObjectPools();
    }

    // Checks player and trigger position and updates platform generation
    private void Update()
    {
        if (player == null || infiniteTrigger == null) return;

        int currentZPos = Mathf.FloorToInt(player.position.z);

        if (currentZPos >= lastZPos + 10)
        {
            lastZPos = currentZPos;
            GenerateNewPlatformOrPoolObject();
        }
    }

    // Fetches an inactive object from a specified pool, or creates a new one if necessary
    private GameObject GetPooledObject(List<GameObject> pool)
    {
        foreach (var pooledObject in pool)
        {
            if (!pooledObject.activeInHierarchy)
            {
                return pooledObject;
            }
        }

        GameObject newObj = Instantiate(pool[0], currentLevel.transform);
        pool.Add(newObj);
        return newObj;
    }

    // Sets a new level and resets the last platform position and Z position
    public void SetLevel(GameObject newLevel)
    {
        currentLevel = newLevel;
        lastZPos = Mathf.FloorToInt(infiniteTrigger.transform.position.z);
    }

    // Sets up the level and infinite trigger variables, and logs an error if they can't be found
    private void SetupLevelAndTrigger()
    {
        infiniteTrigger = GameObject.FindGameObjectWithTag("InfTrigger");
        currentLevel = GameObject.FindGameObjectWithTag("Level");

        if (infiniteTrigger == null)
            Debug.LogError("No GameObject found with InfTrigger tag on Start.");

        if (currentLevel == null)
            Debug.LogError("No GameObject found with Level tag on Start.");
    }

    // Creates the object pools for platforms and pools
    private void CreateObjectPools()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddObjectToPool(platformPool, platformPrefab);
            AddObjectToPool(poolPool, poolPrefab);
        }
    }

    // Adds an object to a specified pool
    private void AddObjectToPool(List<GameObject> pool, GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, currentLevel.transform);
        newObject.SetActive(false);
        pool.Add(newObject);
    }

    // Generates a new platform or pool object and activates it
    private void GenerateNewPlatformOrPoolObject()
    {
        GameObject newObj;
        if (platformCount >= 5)
        {
            newObj = GetPooledObject(poolPool);
            platformCount = 0;
        }
        else
        {
            newObj = GetPooledObject(platformPool);
            platformCount++;
        }

        float platformOffset = infiniteTrigger.transform.position.z - 10f;
        Vector3 newPos = new Vector3(0, 0, Mathf.Round(player.position.z + platformOffset));
        ActivateNewObject(newObj, newPos);
    }

    // Sets the position of a new object, parent it to the current level, and activates it
    private void ActivateNewObject(GameObject newObj, Vector3 newPos)
    {
        newObj.transform.position = newPos;
        newObj.transform.SetParent(currentLevel.transform, false);
        newObj.SetActive(true);
    }
}