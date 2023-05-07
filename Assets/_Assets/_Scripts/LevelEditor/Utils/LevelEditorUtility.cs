using UnityEngine;

public static class LevelEditorUtility
{
    public static void SaveLevel(GameObject levelParent, int platformCount, int levelNumber, LevelData levelData)
    {
        levelData.PlatformCount = platformCount;
        levelData.Positions = new Vector3[platformCount]; // To initialize the Positions array
        levelData.Rotations = new Quaternion[platformCount]; // To initialize the Rotations array

        for (int i = 0; i < platformCount; i++)
        {
            Transform child = levelParent.transform.GetChild(i);
            levelData.Positions[i] = child.position;// To store positions
            levelData.Rotations[i] = child.rotation; // To store rotations
        }

        // Save the LevelData scriptable object to an asset
        UnityEditor.AssetDatabase.SaveAssets();
    }
    public static LevelData LoadLevel(int levelNumber)
    {
        // Load the LevelData scriptable object from the assets
        return Resources.Load<LevelData>($"LevelData_{levelNumber}");
    }
}