using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private int platformCount;
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private GameObject platform;

    private GameObject prefabToInstantiate;
    private GameObject levelParent;

    // Setup level in Start method
    private void Start()
    {
        levelParent = GameObject.FindGameObjectWithTag("Level");
        SetupLevel();
    }

    // Instantiate platform objects in SetupLevel method
    private void SetupLevel()
    {
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < platformCount; i++)
        {
            Instantiate(platform, spawnPosition, Quaternion.identity, levelParent.transform);
            spawnPosition += new Vector3(0, 0, 10);
        }
    }

    // Handle input in Update method
    private void Update()
    {
        HandleInput();
    }

    // Handle mouse input and object instantiation
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && prefabToInstantiate != null && !IsMouseOverUI())
        {
            Vector3 worldPos = GetWorldMousePosition(prefabToInstantiate.tag);

            if (worldPos != Vector3.zero)
            {
                Instantiate(prefabToInstantiate, worldPos, Quaternion.identity, levelParent.transform);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DeleteObjectAtMousePosition();
        }
    }

    // Get the world mouse position based on prefab tag
    public Vector3 GetWorldMousePosition(string tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 position = hit.point;

            if (tag == "Pool" || tag == "Platform" || tag == "Obstacle")
            {
                position.y = 0f;
                position.x = 0f;
                position.z = Mathf.Round(position.z / 10f) * 10f; // round Z position to nearest multiple of 10
            }
            else
            {
                position.y = 0.5f;
            }

            return position;
        }
        return Vector3.zero;
    }

    // Set prefab to instantiate based on index
    public void SetPrefabToInstantiate(int index)
    {
        prefabToInstantiate = itemPrefabs[index];
    }

    // Delete object at mouse position
    private void DeleteObjectAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Plane")) return; // Do not delete the plane object
            Destroy(hit.transform.gameObject);
        }
    }

    // Reset level by deleting all child objects except platform
    public void ResetLevel(GameObject level)
    {
        int childCount = level.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = level.transform.GetChild(i);
            if (child.CompareTag("Platform")) continue;
            Destroy(child.gameObject);
        }
    }

    // Check if mouse is over UI
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
