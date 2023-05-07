using UnityEngine;

public class ItemButtonController : MonoBehaviour
{
    private int index;
    private LevelEditorManager editorManager;

    private void Start()
    {
        editorManager = FindObjectOfType<LevelEditorManager>();

        index = transform.GetSiblingIndex(); // The order of the objects should be the same as level editor item Prefabs list.
    }

    public void ButtonClicked()
    {
        editorManager.SetPrefabToInstantiate(index);
    }
}
