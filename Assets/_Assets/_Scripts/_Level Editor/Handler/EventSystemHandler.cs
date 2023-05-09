using System;
using UnityEngine;

public class EventSystemHandler : MonoBehaviour, IEventHandler
{
    public event Action<Vector3> OnLeftClick;
    public event Action OnRightClick, OnMiddleClick, OnEscapeClick;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnLeftClick?.Invoke(Input.mousePosition);
        else if (Input.GetMouseButtonDown(1)) OnRightClick?.Invoke();
        else if (Input.GetMouseButtonDown(2)) OnMiddleClick?.Invoke();
        else if (Input.GetKeyDown(KeyCode.Escape)) OnEscapeClick?.Invoke();
    }
}
