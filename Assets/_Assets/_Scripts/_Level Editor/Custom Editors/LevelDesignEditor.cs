using UnityEditor;
using UnityEngine;
using System.IO;


public class LevelDesignEditor : EditorWindow
{
    private GameObject prefab;
    private GameObject instance;

    [MenuItem("Window/Level Editor Tool")]
    public static void ShowWindow()
    {
        GetWindow<LevelDesignEditor>("Level Editor Tool");
    }

    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("LevelEditorTool", GUILayout.Width(50));
            prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
            EditorGUILayout.EndHorizontal();

            if (prefab != null && !prefab.scene.IsValid())
            {
                if (GUILayout.Button("Open Level in Scene"))
                {
                    if (EditorUtility.DisplayDialog("Open Level", "Are you sure you want to open this Level? The existing level will be deleted!", "Yes", "No"))
                    {
                        ClearLevel();
                        OpenPrefabInScene();
                    }
                }

                if (instance != null)
                {
                    if (GUILayout.Button("Save Changes"))
                    {
                        SavePrefabChanges();
                    }
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("This tool can only be used in play mode.");
        }
    }

    private void ClearLevel()
    {
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        DestroyImmediate(level);
    }

    private void OpenPrefabInScene()
    {
        if (instance != null)
        {
            DestroyImmediate(instance);
        }

        instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        Selection.activeGameObject = instance;

        // Update the level reference in the LevelEditorManager
        LevelEditorManager levelEditorManager = FindObjectOfType<LevelEditorManager>();
        if (levelEditorManager != null)
        {
            //levelEditorManager.SetLevel(instance);
        }
    }


    private void SavePrefabChanges()
    {
        // Get the path to the "Levels" folder
        string levelsFolderPath = "Assets/_Assets/Prefabs/Levels/";

        // Get the name of the prefab
        string prefabName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(prefab));

        // Construct the path for the new prefab location
        string newPath = Path.Combine(levelsFolderPath, prefabName + ".prefab");

        // Save the prefab at the new location
        PrefabUtility.SaveAsPrefabAsset(instance, newPath);

        // Destroy the instance in the scene
        DestroyImmediate(instance);
    }

}
