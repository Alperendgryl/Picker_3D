using UnityEngine;
using UnityEngine.EventSystems;

public class GridRenderer : MonoBehaviour
{
    [SerializeField] private float gridSize = 10f;
    [SerializeField] private float gridHeight = 0.1f;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Color lineColor = Color.green;
    [SerializeField] private Material lineMaterial;

    private PositionUtility positionUtility;
    private LineRenderer lineRenderer;

    private void Start()
    {
        positionUtility = FindObjectOfType<PositionUtility>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 8;
        lineRenderer.material = lineMaterial;
    }

    private void Update()
    {
        if (!IsMouseOverUI())
        {
            Vector3 worldPos = positionUtility.GetWorldMousePosition("Gizmos", Camera.main);
            if (worldPos != Vector3.zero)
            {
                worldPos.y += gridHeight;
                worldPos.x = Mathf.Round(worldPos.x / gridSize) * gridSize;
                worldPos.z = Mathf.Round(worldPos.z / gridSize) * gridSize;
                DrawSquare(worldPos);
            }
        }
    }

    private void DrawSquare(Vector3 center)
    {
        float halfSize = gridSize / 2;

        Vector3[] vertices = new Vector3[8]
        {
            center + new Vector3(-halfSize, 0, -halfSize),
            center + new Vector3(halfSize, 0, -halfSize),
            center + new Vector3(halfSize, 0, halfSize),
            center + new Vector3(-halfSize, 0, halfSize),
            center + new Vector3(-halfSize, 0, -halfSize),
            center + new Vector3(halfSize, 0, halfSize),
            center + new Vector3(halfSize, 0, -halfSize),
            center + new Vector3(-halfSize, 0, halfSize)
        };

        lineRenderer.SetPositions(vertices);
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
