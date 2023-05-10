using System;
using UnityEngine;

public class GameEventHandler : MonoBehaviour, IGameEventHandler
{
    public event Action OnLevelStarted, OnLevelEnd, OnLevelRestart, OnLevelWin, OnLevelFailed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnLevelStarted?.Invoke(); //|| Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began 

        //if (LevelEnded) OnLevelEnd?.Invoke();

        //if (LevelRestarded) OnLevelRestart?.Invoke();

        //if (LevelWin) OnLevelWin?.Invoke();

        //if (LevelFailed) OnLevelFailed?.Invoke();
    }
}
