using UnityEditor;
using UnityEngine;

public class LevelCompleteCountsEditor : EditorWindow
{
    private int levelNumber = 1;
    private int levelCompleteCount = 0;

    [MenuItem("Window/Level Complete Counts")]
    public static void ShowWindow()
    {
        GetWindow<LevelCompleteCountsEditor>("Level Complete Counts");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Complete Counts Editor", EditorStyles.boldLabel);

        levelNumber = EditorGUILayout.IntField("Level Number", levelNumber);
        levelCompleteCount = EditorGUILayout.IntField("Level Complete Count", levelCompleteCount);

        if (GUILayout.Button("Load Level Complete Count"))
        {
            LoadLevelCompleteCount();
        }

        if (GUILayout.Button("Save Level Complete Count"))
        {
            SaveLevelCompleteCount();
        }
    }

    private void LoadLevelCompleteCount()
    {
        levelCompleteCount = PlayerPrefs.GetInt($"Level{levelNumber}_CompleteCount", 0);
    }

    private void SaveLevelCompleteCount()
    {
        PlayerPrefs.SetInt($"Level{levelNumber}_CompleteCount", levelCompleteCount);
        PlayerPrefs.Save();
    }
}
