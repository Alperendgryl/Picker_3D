using UnityEngine;

public interface IPositionHandler
{
    GameObject GetObjectAtMousePosition();
    public Vector3 GetWorldMousePosition(string tag, Camera camera);
    public Vector3 AdjustPositionOfObject(string tag, Vector3 position);
    public Ray GetRayHit();
}
