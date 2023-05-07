using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector3> OnLeftClick;
    public static event Action OnRightClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClick?.Invoke(Input.mousePosition);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            OnRightClick?.Invoke();
        }
    }
}
