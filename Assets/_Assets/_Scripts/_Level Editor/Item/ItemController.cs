using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    private int index;
    private LevelEditorManager editorManager;

    [Header("Button")]
    private Button button;
    private Color normalColor;
    [SerializeField] private Color selectedColor;

    private void Start()
    {
        editorManager = FindObjectOfType<LevelEditorManager>();
        button = GetComponent<Button>();
        normalColor = button.image.color;
        index = transform.GetSiblingIndex(); // The order of the objects should be the same as level editor item Prefabs list.
    }

    public void ButtonClicked()
    {
        editorManager.SetPrefabToInstantiate(index);
        SetSelected();
    }

    private void SetSelected()
    {
        foreach (var itemController in FindObjectsOfType<ItemController>()) // Deselect other buttons
        {
            if (itemController != this)
            {
                itemController.Deselect();
            }
        }
        button.image.color = selectedColor; // Set the button's color to the selected color
    }

    public void Deselect()
    {
        button.image.color = normalColor; // Set the button's color back to the original color
    }
}
