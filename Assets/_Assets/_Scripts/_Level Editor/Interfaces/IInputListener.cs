using System;
using UnityEngine;

public interface IInputListener
{
    event Action<Vector3> OnLeftClick;
    event Action OnRightClick;
    event Action OnMiddleClick;
}
