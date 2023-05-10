using System;
public interface IGameEventHandler
{
    event Action OnLevelStarted;
    event Action OnLevelEnd;
    event Action OnLevelRestart;
    event Action OnLevelWin;
    event Action OnLevelFailed;
}
