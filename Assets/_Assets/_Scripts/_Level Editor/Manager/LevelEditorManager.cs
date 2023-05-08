using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorManager : MonoBehaviour
{
    private GameObject prefabToInstantiate;

    private LevelHandler levelHandler;
    private InputHandler levelInteraction;
    private IMouseInput inputManager;

    public IMouseInput InputManager // Property injection
    {
        get { return inputManager; }
        set { inputManager = value; }
    }

    private void Start()
    {
        if (inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        levelHandler = GetComponent<LevelHandler>();
        levelInteraction = GetComponent<InputHandler>();

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void HandleLeftClick(Vector3 mousePosition)
    {
        if (prefabToInstantiate != null && !IsMouseOverUI())
        {
            Vector3 worldPos = levelInteraction.GetWorldMousePosition(prefabToInstantiate.tag, Camera.main);

            if (worldPos != Vector3.zero)
            {
                Instantiate(prefabToInstantiate, worldPos, Quaternion.identity, levelInteraction.level.transform);
            }
        }
    }

    private void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        levelInteraction.DeleteObjectAtMousePosition(ray);
    }

    private void HandleMiddleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        levelInteraction.RotateObjectAtMousePosition(ray);
    }

    public void SetPrefabToInstantiate(int index)
    {
        prefabToInstantiate = levelHandler.itemPrefabs[index];
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void SubscribeEvents()
    {
        inputManager.OnLeftClick += HandleLeftClick;
        inputManager.OnRightClick += HandleRightClick;
        inputManager.OnMiddleClick += HandleMiddleClick;
    }

    private void UnsubscribeEvents()
    {
        inputManager.OnLeftClick -= HandleLeftClick;
        inputManager.OnRightClick -= HandleRightClick;
        inputManager.OnMiddleClick -= HandleMiddleClick;
    }
}
