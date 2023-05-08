using UnityEngine;

public class DependencyInjection : MonoBehaviour
{
    private void Start()
    {
        IMouseInput inputManager = FindObjectOfType<InputManager>();
        LevelEditorManager levelEditorManager = FindObjectOfType<LevelEditorManager>();
        levelEditorManager.InputManager = inputManager; // Set the InputManager property for the LevelEditorManager instance
    }
}
