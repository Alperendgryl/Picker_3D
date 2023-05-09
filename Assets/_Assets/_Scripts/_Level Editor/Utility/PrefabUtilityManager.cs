using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabUtilityManager : MonoBehaviour
{
    private int currentLevelCount;

    private void Awake()
    {
        currentLevelCount = PlayerPrefs.GetInt("CurrentLevelCount", 1);
    }

    public void SaveAsPrefab()
    {
#if UNITY_EDITOR
        GameObject level = GameObject.FindGameObjectWithTag("Level");

        if (level != null)
        {
            string levelsFolderPath = "Assets/_Assets/Prefabs/Levels/";
            string prefabName = "Level" + currentLevelCount;
            string prefabPath = Path.Combine(levelsFolderPath, prefabName + ".prefab");

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(level, prefabPath);
            Debug.Log("Prefab saved at: " + prefabPath);

            // Increment the current level count and save it to PlayerPrefs
            currentLevelCount++;
            PlayerPrefs.SetInt("CurrentLevelCount", currentLevelCount);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Level' found.");
        }
#endif
    }
}
