using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    GameObject IInputHandler.level { get; set; }

    private IPositionHandler positionUtility;

    private PrefabSelectionManager psm;
    private UIManager uiManager;
    private DragAndDropUtility dragAndDropUtility;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        positionUtility = FindObjectOfType<PositionUtility>();
        psm = FindObjectOfType<PrefabSelectionManager>();
        dragAndDropUtility = new DragAndDropUtility(positionUtility, uiManager);
    }
    private void Update()
    {
        MoveObject();
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
        foreach (var objectController in FindObjectsOfType<ObjectButtonManager>())
        {
            objectController.SetButtonDeselected();
        }
        psm.SetPrefabAsNull();
    }

    public void MoveObject()
    {
        dragAndDropUtility.Update();
    }
}
