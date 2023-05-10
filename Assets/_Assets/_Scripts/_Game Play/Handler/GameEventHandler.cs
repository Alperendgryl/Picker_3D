using System;
using UnityEngine;

public class GameEventHandler : IGameEventHandler
{
    public event Action OnLevelStarted, OnLevelEnd, OnLevelRestart, OnLevelWin, OnLevelFailed;

    public void TriggerLevelStarted() => OnLevelStarted?.Invoke();
    public void TriggerLevelEnd() => OnLevelEnd?.Invoke();
    public void TriggerLevelRestart() => OnLevelRestart?.Invoke();
    public void TriggerLevelWin() => OnLevelWin?.Invoke();
    public void TriggerLevelFailed() => OnLevelFailed?.Invoke();
}
