using UnityEngine;

public class GameManager : MonoBehaviour, IGameLevelHandler
{
    private GUIManager guiManager;
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
        guiManager = FindObjectOfType<GUIManager>();
        pickerController = FindObjectOfType<PickerController>();
    }

    #region Handle Clicks

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

    public void StartLevel()
    {
        //START LEVEL LOGIC
        pickerController.MovePicker();
        guiManager.StartLevelAnim();
    }

    public void EndLevel()
    {
        //END LEVEL LOGIC
        pickerController.StopPicker();

    }

    public void RestartLevel()
    {
        pickerController.RestartPickerPos();
    }

    public void WinLevel()
    {
        pickerController.RestartPickerPos();
    }

    public void FailLevel()
    {
        pickerController.RestartPickerPos();
    }
    #endregion
}