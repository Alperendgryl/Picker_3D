using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 initialPosition;
    private Material initialMaterial;
    private Material highlightMaterial;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.position;
        initialMaterial = GetComponent<Renderer>().material;
        highlightMaterial = new Material(Shader.Find("Standard"));
        highlightMaterial.color = Color.yellow; // Set the highlight color to yellow or any other color you prefer
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    isDragging = true;
                    GetComponent<Renderer>().material = highlightMaterial;
                }
            }
        }

        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                transform.position = new Vector3(hit.point.x, hit.point.y, initialPosition.z);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            GetComponent<Renderer>().material = initialMaterial;
        }
    }
}
