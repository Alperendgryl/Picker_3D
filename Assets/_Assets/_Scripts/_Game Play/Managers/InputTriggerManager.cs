using UnityEngine;

public class InputTriggerManager : MonoBehaviour
{
    private GameEventHandler gameEventHandler;

    private void Start()
    {
        gameEventHandler = FindObjectOfType<GameManager>().GameEventHandler;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameEventHandler.TriggerLevelStarted();
        }

        // Add other input handling and event triggers here
    }
}
