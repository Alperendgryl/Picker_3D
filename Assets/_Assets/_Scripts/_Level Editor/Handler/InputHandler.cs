using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    GameObject IInputHandler.level { get; set; }

    public Vector3 GetWorldMousePosition(string tag, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 position = hit.point;
            return AdjustPositionBasedOnTag(tag, position);
        }
        return Vector3.zero;
    }

    public void DeleteObjectAtMousePosition(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.transform.CompareTag("Plane")) Destroy(hit.transform.gameObject);
        }
    }

    public void RotateObjectAtMousePosition(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.transform.CompareTag("Plane")) hit.transform.Rotate(Vector3.up, 45f);
        }
    }

    private Vector3 AdjustPositionBasedOnTag(string tag, Vector3 position)
    {
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

}
