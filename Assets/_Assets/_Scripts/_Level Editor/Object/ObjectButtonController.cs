using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ObjectButtonController : MonoBehaviour //Attached to each button
{
    private PrefabSelectionManager psm;

    [SerializeField] private Color selectedColor;
    private int buttonIndex;
    private Button button;
    private Color normalColor;

    private void Start()
    {
        psm = FindObjectOfType<PrefabSelectionManager>();

        button = GetComponent<Button>();
        normalColor = button.image.color;
        buttonIndex = transform.GetSiblingIndex();
    }

    public void ButtonClicked() //buttons onClick()
    {
        psm.SetPrefabToInstantiate(buttonIndex);
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
        button.image.color = normalColor;
    }
}
