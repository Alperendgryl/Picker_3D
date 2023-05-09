using UnityEngine;

public class DragAndDropUtility
{
    private IPositionHandler positionUtility;

    private GameObject selectedObject;
    private Material initialMaterial;
    private Material highlightMaterial;
    public DragAndDropUtility(IPositionHandler positionUtility)
    {
        this.positionUtility = positionUtility;
        highlightMaterial = new Material(Shader.Find("Standard"));
        highlightMaterial.color = Color.yellow;
    }

    public void Update()
    {
        if (UIUtility.IsMouseOverUI()) return;

        if (selectedObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(positionUtility.GetRayHit(), out RaycastHit hit, Mathf.Infinity))
                {
                    selectedObject = hit.collider.gameObject;
                    if (selectedObject.CompareTag("Plane"))
                    {
                        selectedObject = null;
                        return;
                    }
                    Renderer objectRenderer = selectedObject.GetComponent<Renderer>();
                    if (objectRenderer != null)
                    {
                        initialMaterial = objectRenderer.material;
                        objectRenderer.material = highlightMaterial;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(positionUtility.GetRayHit(), out RaycastHit hit, Mathf.Infinity))
                {
                    string tag = selectedObject.tag;
                    Vector3 newPosition = new Vector3(hit.point.x, Mathf.Max(hit.point.y, 0f), hit.point.z);
                    Vector3 adjustedPosition = positionUtility.AdjustPositionOfObject(tag, newPosition);
                    selectedObject.transform.position = adjustedPosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Renderer objectRenderer = selectedObject.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    objectRenderer.material = initialMaterial;
                }
                selectedObject = null;
            }
        }
    }

}
