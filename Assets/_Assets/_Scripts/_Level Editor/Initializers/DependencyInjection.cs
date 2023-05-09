using UnityEngine;

public class DependencyInjection : MonoBehaviour
{
    private void Start()
    {
        IEventHandler inputManager = FindObjectOfType<EventSystemHandler>();
        LevelEditorManager levelEditorManager = FindObjectOfType<LevelEditorManager>();
        levelEditorManager.InputListener = inputManager; // Set the InputListener property for the LevelEditorManager instance
    }
}
