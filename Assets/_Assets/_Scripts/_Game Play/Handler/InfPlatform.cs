using UnityEngine;
using System.Collections.Generic;

public class InfPlatform : MonoBehaviour
{
    private GameObject currentLevel;
    private GameObject infTrigger;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject poolPrefab;
    [SerializeField] private int poolSize = 25;
    [SerializeField] private Transform player;

    private int lastZPos = 0;
    private int platformCount = 0;
    private Vector3 lastPlatformPos;

    private List<GameObject> platformPool;
    private List<GameObject> poolPool;

    private void Awake()
    {
        platformPool = new List<GameObject>();
        poolPool = new List<GameObject>();
    }

    private void Start()
    {
        infTrigger = GameObject.FindGameObjectWithTag("InfTrigger");
        currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (infTrigger == null) Debug.LogError("No GameObject found with InfTrigger tag on Start.");
        if (currentLevel == null) Debug.LogError("No GameObject found with Level tag on Start.");

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newPlatform = Instantiate(platformPrefab, currentLevel.transform);
            newPlatform.SetActive(false);
            platformPool.Add(newPlatform);

            GameObject newPool = Instantiate(poolPrefab, currentLevel.transform);
            newPool.SetActive(false);
            poolPool.Add(newPool);
        }
    }

    private void Update()
    {
        if (player == null || infTrigger == null) return;

        int currentZPos = Mathf.FloorToInt(player.position.z);

        if (currentZPos >= lastZPos + 10)
        {
            lastZPos = currentZPos;

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

            float platformOffset = infTrigger.transform.position.z - 10f;
            Vector3 newPos = new Vector3(0, 0, Mathf.Round(player.position.z + platformOffset));
            newObj.transform.position = newPos;

            newObj.transform.SetParent(currentLevel.transform, false);

            newObj.SetActive(true);
            lastPlatformPos = newPos;
        }
    }


    private GameObject GetPooledObject(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        GameObject newObj = Instantiate(pool[0], currentLevel.transform);
        pool.Add(newObj);
        return newObj;
    }

    public void SetLevel(GameObject newLevel)
    {
        currentLevel = newLevel;
        lastPlatformPos = infTrigger.transform.position;
        lastZPos = Mathf.FloorToInt(lastPlatformPos.z);
    }
}
