using UnityEngine;

public interface IObjectHandler
{
    GameObject GetObjectAtMousePosition();
    void SelectObject(GameObject obj);
    void DeselectObject();
    void UpdateSelectedObjectPosition(IInputHandler inputHandler);
}
