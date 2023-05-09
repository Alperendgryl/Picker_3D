using UnityEngine;

public interface IInputHandler
{
    GameObject level { get; set; }
    void DeleteObject();
    void RotateObject();
    void SelectObject(GameObject obj);
    void DeselectObject();
    void MoveObject();
}
