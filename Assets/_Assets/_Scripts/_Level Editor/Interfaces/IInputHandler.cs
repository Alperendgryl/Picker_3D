using UnityEngine;

public interface IInputHandler
{
    GameObject level { get; set; }
    void DeleteObject();
    void RotateObject();
    void SelectObject(GameObject prefabToInstantiate);
    void DeselectObject();
    void MoveObject();
    void InstantiateObject(GameObject prefabToInstantiate);
    void ResetObjectButtons();
}
