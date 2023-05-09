using UnityEngine;

public class PositionUtility : MonoBehaviour, IPositionHandler
{
    public Vector3 GetWorldMousePosition(string tag, Camera camera)
    {
        if (Physics.Raycast(GetRayHit(), out RaycastHit hit))
        {
            Vector3 position = hit.point;
            return AdjustPositionOfObject(tag, position);
        }
        return Vector3.zero;
    }

    public Vector3 AdjustPositionOfObject(string tag, Vector3 position)
    {
        if (tag == "Platform" || tag == "LevelEnd" || tag == "Pool")
        {
            position.x = 0f;
            position.y = 0f;
            position.z = Mathf.Round(position.z / 10f) * 10f;
        }
        else
        {
            position.y = 0.5f;
        }
        return position;
    }

    public GameObject GetObjectAtMousePosition()
    {
        if (Physics.Raycast(GetRayHit(), out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Plane"))
            {
                return null;
            }

            return hit.collider.gameObject;
        }

        return null;
    }
    public Ray GetRayHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray;
    }
}
