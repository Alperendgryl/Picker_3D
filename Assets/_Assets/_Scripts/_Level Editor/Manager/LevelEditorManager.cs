using System;
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    private PrefabSelectionManager prefabSelectionManager;
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
        prefabSelectionManager = FindObjectOfType<PrefabSelectionManager>();

        inputHandler.level = levelHandler.level;
    }
    #endregion
    #region Handle Clicks
    private GameObject prefabToInstantiate;
    private void HandleLeftClick(Vector3 mousePosition)
    {
        if (UIUtility.IsMouseOverUI()) return;

        prefabToInstantiate = prefabSelectionManager.GetPrefabToInstantiate();
        if (prefabToInstantiate != null) inputHandler.InstantiateObject(prefabToInstantiate);
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
        prefabToInstantiate = null;
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
