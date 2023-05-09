using UnityEngine;

public interface IInputHandler
{
    GameObject level { get; set; }
    Vector3 GetWorldMousePosition(string tag, Camera camera);
    void DeleteObjectAtMousePosition(Ray ray);
    void RotateObjectAtMousePosition(Ray ray);
}
