using UnityEditor;
using UnityEngine;
using System.IO;

public class PrefabUtilityManager : MonoBehaviour
{
    public void SaveAsPrefab()
    {
#if UNITY_EDITOR
        GameObject level = GameObject.FindGameObjectWithTag("Level");

        if (level != null)
        {
            string levelsFolderPath = "Assets/_Assets/Prefabs/Levels/";
            string prefabName = "LevelPrefab";
            string prefabPath = Path.Combine(levelsFolderPath, prefabName + ".prefab");

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(level, prefabPath);
            Debug.Log("Prefab saved at: " + prefabPath);
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Level' found.");
        }
#endif
    }
}
