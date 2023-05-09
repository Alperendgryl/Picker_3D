using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorManager : MonoBehaviour
{
    private GameObject prefabToInstantiate;

    #region External Scripts
    private LevelHandler levelHandler;
    private IInputHandler inputHandler;
    private IEventHandler eventHandler;
    private IPositionHandler positionUtility;
    #endregion

    #region Property injection
    public GameObject Level
    {
        get { return inputHandler.level; }
        set { inputHandler.level = value; }
    }

    public IInputHandler InputHandler
    {
        get { return inputHandler; }
        set { inputHandler = value; }
    }

    public IEventHandler EventHandler
    {
        get { return eventHandler; }
        set { eventHandler = value; }
    }

    public IPositionHandler PositionHandler
    {
        get { return positionUtility; }
        set { positionUtility = value; }
    }
    #endregion

    #region Lifecycle 

    private void Start()
    {
        InitializeHandlers();
        SubscribeEvents();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetSelectedPrefabAndDeselectButtons();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void InitializeHandlers()
    {
        if (eventHandler == null)
        {
            eventHandler = FindObjectOfType<EventSystemHandler>();
        }
        positionUtility = GetComponent<PositionUtility>();
        levelHandler = GetComponent<LevelHandler>();
        inputHandler = GetComponent<InputHandler>();
        inputHandler.level = levelHandler.level;
    }
    #endregion

    #region Handle Click
    private void HandleLeftClick(Vector3 mousePosition)
    {
        if (UIUtility.IsMouseOverUI())
        {
            return;
        }

        if (prefabToInstantiate != null)
        {
            InstantiatePrefabAtMousePosition();
        }
    }

    private void HandleRightClick()
    {
        inputHandler.DeleteObject();
    }

    private void HandleMiddleClick()
    {
        inputHandler.RotateObject();
    }
    #endregion

    #region Prefab Handlers

    private void InstantiatePrefabAtMousePosition()
    {
        if (!UIUtility.IsMouseOverUI())
        {
            Vector3 worldPos = positionUtility.GetWorldMousePosition(prefabToInstantiate.tag, Camera.main);

            if (worldPos != Vector3.zero)
            {
                // Check if there is already an object with the same tag at the world position
                Collider[] colliders = Physics.OverlapSphere(worldPos, 0.1f);
                bool shouldInstantiate = true;

                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Platform") || collider.gameObject.CompareTag("LevelEnd") || collider.gameObject.CompareTag("Pool"))
                    {
                        shouldInstantiate = false;
                        break;
                    }
                }

                if (shouldInstantiate)
                {
                    GameObject instantiatedObject = Instantiate(prefabToInstantiate, worldPos, Quaternion.identity, inputHandler.level.transform);
                    if (instantiatedObject == null)
                    {
                        Debug.LogError("Failed to instantiate prefab: " + prefabToInstantiate.name);
                        return;
                    }
                    Debug.Log("Instantiated: " + prefabToInstantiate.name);
                }
                else
                {
                    Debug.Log("An object already exists at the position!");
                }
            }
        }
    }

    public void ResetSelectedPrefabAndDeselectButtons()
    {
        prefabToInstantiate = null;
        foreach (var objectController in FindObjectsOfType<ObjectButtonController>())
        {
            objectController.SetButtonDeselected();
        }
    }
    #endregion

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