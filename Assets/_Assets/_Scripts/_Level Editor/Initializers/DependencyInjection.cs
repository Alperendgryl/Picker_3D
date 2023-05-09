using UnityEngine;

public class DependencyInjection : MonoBehaviour
{
    private void Start()
    {
        IEventHandler inputManager = FindObjectOfType<EventSystemHandler>();
        LevelEditorManager levelEditorManager = FindObjectOfType<LevelEditorManager>();
        levelEditorManager.EventHandler = inputManager; // Set the EventHandler property for the LevelEditorManager instance
    }
}
