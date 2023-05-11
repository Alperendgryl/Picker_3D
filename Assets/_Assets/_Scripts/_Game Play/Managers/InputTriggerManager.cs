using UnityEngine;
using UnityEngine.EventSystems;

public class InputTriggerManager : MonoBehaviour
{
    private GameEventHandler gameEventHandler;

    private void Start()
    {
        gameEventHandler = FindObjectOfType<GameManager>().GameEventHandler;
    }

    private void Update()
    {
        // Check if the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            gameEventHandler.TriggerLevelStarted();
        }

        // Add other input handling and event triggers here
    }

}
