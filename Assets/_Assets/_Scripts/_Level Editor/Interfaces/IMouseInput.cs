using System;
using UnityEngine;

public interface IMouseInput
{
    event Action<Vector3> OnLeftClick;
    event Action OnRightClick;
    event Action OnMiddleClick;
}
