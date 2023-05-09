using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorManager : MonoBehaviour
{
    private GameObject prefabToInstantiate;
    private GameObject selectedObj;

    #region External Scripts
    private LevelHandler levelHandler;
    private IInputHandler inputHandler;
    private IObjectHandler objectHandler;
    private IEventHandler eventHandler;
    #endregion

    #region Property injection
    public GameObject Level
    {
        get { return inputHandler.level; }
        set { inputHandler.level = value; }
    }

    public IEventHandler InputListener
    {
        get { return eventHandler; }
        set { eventHandler = value; }
    }
    #endregion

    private void Awake()
    {
        objectHandler = new ObjectHandler(selectedObj);
    }

    private void Start()
    {
        InitializeHandlers();
        SubscribeEvents();
    }

    private void Update()
    {
        if (selectedObj != null)
        {
            objectHandler.UpdateSelectedObjectPosition(inputHandler);
        }
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    #region Handle Click
    private void HandleLeftClick(Vector3 mousePosition)
    {
        if (IsMouseOverUI())
        {
            return;
        }

        if (selectedObj != null)
        {
            objectHandler.UpdateSelectedObjectPosition(inputHandler);
            objectHandler.DeselectObject();
        }
        else
        {
            HandleObjectSelectionOrInstantiation();
        }
    }
    private void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        inputHandler.DeleteObjectAtMousePosition(ray);
    }

    private void HandleMiddleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        inputHandler.RotateObjectAtMousePosition(ray);
    }
    #endregion

    private void HandleObjectSelectionOrInstantiation()
    {
        if (prefabToInstantiate == null)
        {
            GameObject obj = objectHandler.GetObjectAtMousePosition();

            if (obj != null)
            {
                objectHandler.SelectObject(obj);
            }
        }
        else
        {
            InstantiatePrefabAtMousePosition();
        }
    }
    private void InstantiatePrefabAtMousePosition()
    {
        if (!IsMouseOverUI())
        {
            Vector3 worldPos = inputHandler.GetWorldMousePosition(prefabToInstantiate.tag, Camera.main);

            if (worldPos != Vector3.zero)
            {
                GameObject instantiatedObject = Instantiate(prefabToInstantiate, worldPos, Quaternion.identity, inputHandler.level.transform);
                if (instantiatedObject == null)
                {
                    Debug.LogError("Failed to instantiate prefab: " + prefabToInstantiate.name);
                    return;
                }
                Debug.Log("Instantiated: " + prefabToInstantiate.name);
            }
        }
    }

    #region Prefab To Instantiate
    public void SetPrefabToInstantiate(int index)
    {
        prefabToInstantiate = levelHandler.itemPrefabs[index];
    }

    private void UpdatePrefabToInstantiate()
    {
        if (prefabToInstantiate != null)
        {
            int prefabIndex = Array.FindIndex(levelHandler.itemPrefabs, prefab => prefab.name == prefabToInstantiate.name);
            if (prefabIndex >= 0)
            {
                prefabToInstantiate = levelHandler.itemPrefabs[prefabIndex];
            }
        }
        Level = levelHandler.level; // Update the level reference
    }
    #endregion

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void InitializeHandlers()
    {
        if (eventHandler == null)
        {
            eventHandler = FindObjectOfType<EventSystemHandler>();
        }

        levelHandler = GetComponent<LevelHandler>();
        inputHandler = GetComponent<InputHandler>();
        inputHandler.level = levelHandler.level;
    }

    #region Events
    private void SubscribeEvents()
    {
        eventHandler.OnLeftClick += HandleLeftClick;
        eventHandler.OnRightClick += HandleRightClick;
        eventHandler.OnMiddleClick += HandleMiddleClick;
        levelHandler.OnLevelLoaded += UpdatePrefabToInstantiate;
    }

    private void UnsubscribeEvents()
    {
        eventHandler.OnLeftClick -= HandleLeftClick;
        eventHandler.OnRightClick -= HandleRightClick;
        eventHandler.OnMiddleClick -= HandleMiddleClick;
        levelHandler.OnLevelLoaded -= UpdatePrefabToInstantiate;
    }
    #endregion Events
}