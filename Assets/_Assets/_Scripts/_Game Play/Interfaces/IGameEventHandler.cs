using System;
public interface IGameEventHandler
{
    event Action OnLevelStarted;
    event Action OnLevelWin;
    event Action OnLevelFailed;

    void TriggerLevelStarted();
    void TriggerLevelWin();
    void TriggerLevelFailed();
}
