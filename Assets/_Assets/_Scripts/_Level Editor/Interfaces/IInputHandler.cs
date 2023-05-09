using UnityEngine;

public interface IInputHandler
{
    GameObject level { get; set; }
    void DeleteObject();
    void RotateObject();
    void InstantiateObject();
    void ResetObjectButtons();
}
