using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    GameObject IInputHandler.level { get; set; }

    private GameObject selectedObj;
    private Material originalMaterial;
    private Material highlightMaterial;

    public bool isDragging = false;
    public Vector3 offset;
    public Vector3 originalPosition;

    private IPositionHandler positionHandler;

    private void Awake()
    {
        positionHandler = GetComponent<IPositionHandler>();
        highlightMaterial = new Material(Shader.Find("Standard"));
        highlightMaterial.color = Color.yellow;
    }

    public void DeleteObject()
    {
        if (Physics.Raycast(positionHandler.GetRayHit(), out RaycastHit hit))
        {
            if (!hit.transform.CompareTag("Plane")) Destroy(hit.transform.gameObject);
        }
    }

    public void RotateObject()
    {
        if (Physics.Raycast(positionHandler.GetRayHit(), out RaycastHit hit))
        {
            if (!hit.transform.CompareTag("Plane")) hit.transform.Rotate(Vector3.up, 45f);
        }
    }

    public void SelectObject(GameObject obj)
    {
        if (selectedObj != null)
        {
            DeselectObject();
        }

        selectedObj = obj;
        originalMaterial = obj.GetComponent<Renderer>().material;
        obj.GetComponent<Renderer>().material = highlightMaterial;

        isDragging = true;
        originalPosition = obj.transform.position;
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

    public void MoveObject()
    {
        throw new System.NotImplementedException();
    }
}
