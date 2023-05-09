using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButtonController : MonoBehaviour //Attached to each button
{
    private PrefabSelectionManager prefabSelectionManager;

    [SerializeField] private Color selectedColor;
    private int buttonIndex;
    private Button button;
    private Color normalColor;

    private void Start()
    {
        prefabSelectionManager = FindObjectOfType<PrefabSelectionManager>();

        button = GetComponent<Button>();
        normalColor = button.image.color;
        buttonIndex = transform.GetSiblingIndex(); // The order of the objects should be the same as level editor item Prefabs list.
    }

    public void ButtonClicked() // Attached to the buttons onClick()
    {
        prefabSelectionManager.SetPrefabToInstantiate(buttonIndex);
        SetButtonSelected();
    }

    private void SetButtonSelected()
    {
        foreach (var objectController in FindObjectsOfType<ObjectButtonController>()) // SetButtonDeselected other buttons
        {
            if (objectController != this) objectController.SetButtonDeselected();
        }
        button.image.color = selectedColor; // Set the button's color to the selected color
    }

    public void SetButtonDeselected()
    {
        button.image.color = normalColor; // Set the button's color back to the original color
    }
}
