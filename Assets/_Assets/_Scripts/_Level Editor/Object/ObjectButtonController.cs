using UnityEngine;
using UnityEngine.UI;

public class ObjectButtonController : MonoBehaviour //Attached to each button
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

    public void ButtonClicked() // Attached to the buttons onClick()
    {
        editorManager.SetPrefabToInstantiate(index);
        SetButtonSelected();
    }

    private void SetButtonSelected()
    {
        foreach (var objectController in FindObjectsOfType<ObjectButtonController>()) // SetButtonDeselected other buttons
        {
            if (objectController != this)
            {
                objectController.SetButtonDeselected();
            }
        }
        button.image.color = selectedColor; // Set the button's color to the selected color
    }

    public void SetButtonDeselected()
    {
        button.image.color = normalColor; // Set the button's color back to the original color
    }
}
