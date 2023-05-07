using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int platformCount;
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private GameObject platform;

    private GameObject prefabToInstantiate;
    private GameObject levelParent;

    private GameObject selectedObject;
    private bool isObjectSelected;

    private void Start()
    {
        levelParent = GameObject.FindGameObjectWithTag("Level");
        SetupLevel();
        SubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SetupLevel()
    {
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < platformCount; i++)
        {
            Instantiate(platform, spawnPosition, Quaternion.identity, levelParent.transform);
            spawnPosition += new Vector3(0, 0, 10);
        }
    }

    private void HandleLeftClick(Vector3 mousePosition)
    {
        if (prefabToInstantiate != null && !IsMouseOverUI())
        {
            Vector3 worldPos = GetWorldMousePosition(prefabToInstantiate.tag);

            if (worldPos != Vector3.zero)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject obj = Instantiate(prefabToInstantiate, worldPos, Quaternion.identity, levelParent.transform);
                    if (isObjectSelected)
                    {
                        selectedObject = obj;
                        isObjectSelected = false;
                    }
                }
            }
        }
    }

    private void HandleRightClick()
    {
        DeleteObjectAtMousePosition();
    }

    private void HandleMiddleClick()
    {
        RotateObjectAtMousePosition();
    }

    public Vector3 GetWorldMousePosition(string tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 position = hit.point;

            if (tag == "Platform" || tag == "Obstacle" || tag == "Level End")
            {
                position.x = 0f;
                position.y = 0f;
                position.z = Mathf.Round(position.z / 10f) * 10f;
            }
            else if (tag == "Pool")
            {
                position.x = 0f;
                position.y = 0f;
                position.z = Mathf.Round(position.z / 10f) * 10f - 10f;
            }
            else
            {
                position.y = 0.5f;
            }
            return position;
        }
        return Vector3.zero;
    }

    public void SetPrefabToInstantiate(int index)
    {
        prefabToInstantiate = itemPrefabs[index];
    }

    //private GameObject lastClickedObject;
    //private void DeleteObjectAtMousePosition()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        if (!hit.transform.CompareTag("Plane"))
    //        {
    //            if (hit.transform.gameObject == lastClickedObject) // If the same object is clicked twice, delete it
    //            {
    //                Destroy(hit.transform.gameObject);
    //            }
    //            else
    //            {
    //                lastClickedObject = hit.transform.gameObject; // Set the last clicked object to the new object
    //            }
    //        }
    //    }
    //}

    private void DeleteObjectAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.transform.CompareTag("Plane")) Destroy(hit.transform.gameObject);
        }
    }

    private void RotateObjectAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.transform.CompareTag("Plane")) hit.transform.Rotate(Vector3.up, 45f); ;
        }
    }

    public void ResetLevel()
    {
        foreach (Transform child in levelParent.transform)
        {
            if (child.CompareTag("Platform")) continue;
            Destroy(child.gameObject);
        }
    }

    public void LoadLevel(int levelNumber)
    {
        ResetLevel();
        LevelData levelData = LevelEditorUtility.LoadLevel(levelNumber);

        if (levelData != null)
        {
            for (int i = 0; i < levelData.PlatformCount; i++)
            {
                Vector3 position = levelData.Positions[i];
                Quaternion rotation = levelData.Rotations[i]; // Add this line to get rotations
                Instantiate(platform, position, rotation, levelParent.transform); // Update this line to include rotation
            }
        }
    }

    private bool IsMouseOverUI()// Check if mouse is over UI
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private void SubscribeEvents()
    {
        // Subscribe to events
        InputManager.OnLeftClick += HandleLeftClick;
        InputManager.OnRightClick += HandleRightClick;
        InputManager.OnMiddleClick += HandleMiddleClick;
        LevelManager.OnLevelSelected += LoadLevel;
        LevelManager.OnLevelSelected += level => currentLevel = level;
    }
    private void UnsubscribeEvents()
    {
        // Unsubscribe from events
        InputManager.OnLeftClick -= HandleLeftClick;
        InputManager.OnRightClick -= HandleRightClick;
        InputManager.OnMiddleClick -= HandleMiddleClick;
        LevelManager.OnLevelSelected -= LoadLevel;
        LevelManager.OnLevelSelected -= level => currentLevel = level;
    }
}
