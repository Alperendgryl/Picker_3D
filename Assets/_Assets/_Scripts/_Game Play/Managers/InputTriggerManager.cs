using UnityEngine;
using UnityEngine.EventSystems;

public class InputTriggerManager : MonoBehaviour
{
    private GameEventHandler gameEventHandler;

    private void Start()
    {
        gameEventHandler = GameManager.Instance.GameEventHandler;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            gameEventHandler.TriggerLevelStarted();
        }
    }
}