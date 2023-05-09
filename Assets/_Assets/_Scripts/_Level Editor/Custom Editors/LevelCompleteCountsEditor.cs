using UnityEditor;
using UnityEngine;

public class LevelCompleteCountsEditor : EditorWindow
{
    private int levelNumber = 1;
    private int levelCompleteCount = 0;

    [MenuItem("Window/level Complete Counts")]
    public static void ShowWindow()
    {
        GetWindow<LevelCompleteCountsEditor>("level Complete Counts");
    }

    private void OnGUI()
    {
        GUILayout.Label("level Complete Counts Editor", EditorStyles.boldLabel);

        levelNumber = EditorGUILayout.IntField("level Number", levelNumber);
        levelCompleteCount = EditorGUILayout.IntField("level Complete Count", levelCompleteCount);

        if (GUILayout.Button("Load level Complete Count"))
        {
            LoadLevelCompleteCount();
        }

        if (GUILayout.Button("Save level Complete Count"))
        {
            SaveLevelCompleteCount();
        }
    }

    private void LoadLevelCompleteCount()
    {
        levelCompleteCount = PlayerPrefs.GetInt($"level{levelNumber}_CompleteCount", 0);
    }

    private void SaveLevelCompleteCount()
    {
        PlayerPrefs.SetInt($"level{levelNumber}_CompleteCount", levelCompleteCount);
        PlayerPrefs.Save();
    }
}
