using UnityEngine;

public class ObjectHandler : IObjectHandler
{
    private GameObject selectedObj;
    private Material originalMaterial;
    private Material highlightMaterial;

    public ObjectHandler(GameObject selectedObj)
    {
        this.selectedObj = selectedObj;
        highlightMaterial = new Material(Shader.Find("Standard"));
        highlightMaterial.color = Color.yellow;
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
    }

    public void DeselectObject()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Renderer>().material = originalMaterial;
            selectedObj = null;
        }
    }

    public GameObject GetObjectAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Plane"))
            {
                return null;
            }

            return hit.collider.gameObject;
        }

        return null;
    }

    public void UpdateSelectedObjectPosition(IInputHandler inputHandler)
    {
        Vector3 worldPos = inputHandler.GetWorldMousePosition(selectedObj.tag, Camera.main);
        UpdateObjectPosition(selectedObj, worldPos);
    }

    public void UpdateObjectPosition(GameObject obj, Vector3 newPosition)
    {
        obj.transform.position = newPosition;
    }
}
