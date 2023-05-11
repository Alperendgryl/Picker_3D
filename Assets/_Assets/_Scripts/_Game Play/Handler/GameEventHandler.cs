using System;

public class GameEventHandler : IGameEventHandler
{
    public event Action OnLevelStarted, OnLevelWin, OnLevelFailed;

    public void TriggerLevelStarted() => OnLevelStarted?.Invoke();
    public void TriggerLevelWin() => OnLevelWin?.Invoke();
    public void TriggerLevelFailed() => OnLevelFailed?.Invoke();
}
