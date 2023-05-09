using System;
using UnityEngine;

public interface IEventHandler
{
    event Action<Vector3> OnLeftClick;
    event Action OnRightClick;
    event Action OnMiddleClick;
    event Action OnEscapeClick;
}
