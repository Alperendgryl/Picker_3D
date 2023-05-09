using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    GameObject IInputHandler.level { get; set; }

    private GameObject selectedObj;
    private Material originalMaterial, highlightMaterial;

    public bool isDragging = false;
    public Vector3 offset, originalPosition;

    private IPositionHandler positionUtility;
    private PrefabSelectionManager psm;
    private void Awake()
    {
        positionUtility = FindObjectOfType<PositionUtility>();
        psm = FindObjectOfType<PrefabSelectionManager>();

        highlightMaterial = new Material(Shader.Find("Standard"));
        highlightMaterial.color = Color.yellow;
    }

    public void SelectObject(GameObject prefabToInstantiate)
    {
        if (selectedObj != null) DeselectObject();

        selectedObj = prefabToInstantiate;
        originalMaterial = prefabToInstantiate.GetComponent<Renderer>().material;
        prefabToInstantiate.GetComponent<Renderer>().material = highlightMaterial;

        isDragging = true;
        originalPosition = prefabToInstantiate.transform.position;
    }

    public void DeselectObject()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Renderer>().material = originalMaterial;
            selectedObj = null;
        }
        isDragging = false;
    }

    public void InstantiateObject()
    {
        GameObject prefabToInstantiate = psm.GetPrefabToInstantiate();

        if (prefabToInstantiate != null)
        {
            Vector3 worldPos = positionUtility.GetWorldMousePosition(prefabToInstantiate.tag, Camera.main);

            if (worldPos != Vector3.zero)
            {
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
                    GameObject instantiatedObject = Instantiate(prefabToInstantiate, worldPos, Quaternion.identity, ((IInputHandler)this).level.transform);
                    if (instantiatedObject == null) return;
                    Debug.Log("Instantiated: " + prefabToInstantiate.name);
                }
                else Debug.Log("An object already exists at the position!");
            }
        }
    }

    public void DeleteObject()
    {
        if (Physics.Raycast(positionUtility.GetRayHit(), out RaycastHit hit)) if (!hit.transform.CompareTag("Plane")) Destroy(hit.transform.gameObject);
    }

    public void RotateObject()
    {
        if (Physics.Raycast(positionUtility.GetRayHit(), out RaycastHit hit)) if (!hit.transform.CompareTag("Plane")) hit.transform.Rotate(Vector3.up, 45f);
    }

    public void ResetObjectButtons()
    {
        foreach (var objectController in FindObjectsOfType<ObjectButtonController>())
        {
            objectController.SetButtonDeselected();
        }
        psm.SetPrefabAsNull();
    }

    public void MoveObject()
    {
        //Move the object position logic
    }
}
