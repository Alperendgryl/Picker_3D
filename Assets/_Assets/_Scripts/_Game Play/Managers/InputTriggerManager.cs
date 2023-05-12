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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Check if touch began over a UI GameObject
            if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

            if (touch.phase == TouchPhase.Began) gameEventHandler.TriggerLevelStarted();

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;
                transform.Translate(-touchDeltaPosition.x * Time.deltaTime, 0, -touchDeltaPosition.y * Time.deltaTime);
            }
        }
    }

}