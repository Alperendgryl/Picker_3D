using UnityEngine;
using System.Collections.Generic;

public class InfPlatform : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject poolPrefab;
    public int poolSize = 10;
    public Transform player;

    private int lastZPos = 0;
    private int platformCount = 0;
    private Vector3 lastPlatformPos;

    private List<GameObject> platformPool;
    private List<GameObject> poolPool;

    private void Awake()
    {
        platformPool = new List<GameObject>();
        poolPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newPlatform = Instantiate(platformPrefab);
            newPlatform.SetActive(false);
            platformPool.Add(newPlatform);

            GameObject newPool = Instantiate(poolPrefab);
            newPool.SetActive(false);
            poolPool.Add(newPool);
        }

        // Find the Level GameObject and its child with the LevelEnd tag
        GameObject level = GameObject.FindWithTag("Level");
        if (level != null)
        {
            Transform levelEnd = level.transform.Find("LevelEnd");
            if (levelEnd != null)
            {
                lastPlatformPos = levelEnd.position;
                lastZPos = Mathf.FloorToInt(lastPlatformPos.z);
            }
            else
            {
                Debug.LogError("No child GameObject found with LevelEnd tag.");
            }
        }
        else
        {
            Debug.LogError("No GameObject found with Level tag.");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned in LevelManager script.");
            return;
        }

        int currentZPos = Mathf.FloorToInt(player.position.z);

        if (currentZPos >= 120 && currentZPos >= lastZPos + 10)
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

            Vector3 newPos = lastPlatformPos + new Vector3(0, 0, 10);
            newObj.transform.position = newPos;
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

        GameObject newObj = Instantiate(pool[0]);
        pool.Add(newObj);
        return newObj;
    }
}
