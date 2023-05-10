using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UIAnimationManager uiAnimationController;
    private PickerController pickerController;

    private IGameEventHandler gameEventHandler;

    private void Start()
    {
        InitializeHandlers();
        SubscribeEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void InitializeHandlers()
    {
        gameEventHandler = FindObjectOfType<GameEventHandler>();
        uiAnimationController = FindObjectOfType<UIAnimationManager>();
        pickerController = FindObjectOfType<PickerController>();
    }

    #region Handle Clicks
    private void StartLevel()
    {
        //START LEVEL LOGIC
        pickerController.MovePicker();
        uiAnimationController.StartLevelAnim();
    }

    private void EndLevel()
    {
        //END LEVEL LOGIC
        pickerController.StopPicker();
        uiAnimationController.EndLevelAnim();
    }

    private void RestartLevel()
    {
        //RESTART LEVEL LOGIC
        uiAnimationController.RestartLevelAnim();
    }

    private void WinLevel()
    {
        //WIN LEVEL LOGIC
        uiAnimationController.WinLevelAnim();
    }

    private void FailLevel()
    {
        //FAIL LEVEL LOGIC
        uiAnimationController.FailLevelAnim();
    }

    #endregion

    #region Events
    private void SubscribeEvents()
    {
        gameEventHandler.OnLevelStarted += StartLevel;
        gameEventHandler.OnLevelEnd += EndLevel;
        gameEventHandler.OnLevelRestart += RestartLevel;
        gameEventHandler.OnLevelWin += WinLevel;
        gameEventHandler.OnLevelFailed += FailLevel;
    }
    private void UnsubscribeEvents()
    {
        gameEventHandler.OnLevelStarted -= StartLevel;
        gameEventHandler.OnLevelEnd -= EndLevel;
        gameEventHandler.OnLevelRestart -= RestartLevel;
        gameEventHandler.OnLevelWin -= WinLevel;
        gameEventHandler.OnLevelFailed -= FailLevel;
    }
    #endregion
}