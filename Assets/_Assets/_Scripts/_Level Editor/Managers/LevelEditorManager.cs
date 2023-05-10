using System;
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    private LevelHandler levelHandler;
    private IInputHandler inputHandler;
    private IEventHandler eventHandler;
    public IEventHandler EventHandler
    {
        get { return eventHandler; }
        set { eventHandler = value; }
    }
    #region Lifecycle 
    private void Start()
    {
        InitializeHandlers();
        SubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void InitializeHandlers()
    {
        if (eventHandler == null) eventHandler = FindObjectOfType<EventSystemHandler>();

        levelHandler = FindObjectOfType<LevelHandler>();
        inputHandler = FindObjectOfType<InputHandler>();

        SetLevel(levelHandler.level);
        inputHandler.level = levelHandler.level;
    }
    public void SetLevel(GameObject level)
    {
        if (inputHandler != null)
        {
            inputHandler.level = level;
        }
    }
    #endregion

    #region Handle Clicks
    private void HandleLeftClick(Vector3 mousePosition)
    {
        if (UIUtility.IsMouseOverUI()) return;
        inputHandler.InstantiateObject();
    }

    private void HandleRightClick()
    {
        inputHandler.DeleteObject();
    }

    private void HandleMiddleClick()
    {
        inputHandler.RotateObject();
    }

    private void HandleEscapeClick()
    {
        inputHandler.ResetObjectButtons();
    }
    #endregion

    #region Events
    private void SubscribeEvents()
    {
        eventHandler.OnLeftClick += HandleLeftClick;
        eventHandler.OnRightClick += HandleRightClick;
        eventHandler.OnMiddleClick += HandleMiddleClick;
        eventHandler.OnEscapeClick += HandleEscapeClick;
    }

    private void UnsubscribeEvents()
    {
        eventHandler.OnLeftClick -= HandleLeftClick;
        eventHandler.OnRightClick -= HandleRightClick;
        eventHandler.OnMiddleClick -= HandleMiddleClick;
        eventHandler.OnEscapeClick -= HandleEscapeClick;
    }
    #endregion Events
}
