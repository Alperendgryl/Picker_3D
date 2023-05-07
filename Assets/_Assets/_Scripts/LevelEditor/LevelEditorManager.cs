using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private int platformCount;
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private GameObject platform;

    private GameObject prefabToInstantiate;
    private GameObject levelParent;

    private void Start()
    {
        levelParent = GameObject.FindGameObjectWithTag("Level");
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < platformCount; i++)
        {
            Instantiate(platform, spawnPosition, Quaternion.identity, levelParent.transform);
            spawnPosition += new Vector3(0, 0, 10);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && prefabToInstantiate != null && !IsMouseOverUI())
        {
            Vector3 worldPos = GetWorldMousePosition() + Vector3.up / 2;

            if (worldPos != Vector3.zero)
            {
                if (levelParent != null)
                {
                    Instantiate(prefabToInstantiate, worldPos, Quaternion.identity, levelParent.transform);
                }
                else
                {
                    Debug.LogError("No GameObject found with the 'Platform' tag");
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DeleteObjectAtMousePosition();
        }
    }

    public Vector3 GetWorldMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.transform.tag.Equals("Plane")) // Cannot add to plane object (Only on platform)
            return hit.point;
        }
        return Vector3.zero;
    }

    public void SetPrefabToInstantiate(int index)
    {
        prefabToInstantiate = itemPrefabs[index];
    }

    private void DeleteObjectAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.tag.Equals("Plane")) return; // Cannot delete the plane object (Only objects on platform)
            Destroy(hit.transform.gameObject);
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
