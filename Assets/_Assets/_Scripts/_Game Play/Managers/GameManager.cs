using UnityEngine;

public class GameManager : MonoBehaviour, IGameLevelHandler
{
    private GUIManager guiManager;
    private PickerController pickerController;
    private GameEventHandler gameEventHandler;
    public GameEventHandler GameEventHandler
    {
        get
        {
            if (gameEventHandler == null)
            {
                gameEventHandler = new GameEventHandler();
            }
            return gameEventHandler;
        }
    }

    private void Start()
    {
        InitializeHandlers();
        SubscribeEvents();
    }

    private void InitializeHandlers()
    {
        guiManager = FindObjectOfType<GUIManager>();
        pickerController = FindObjectOfType<PickerController>();
    }

    public void StartLevel()
    {
        pickerController.MovePicker();
        guiManager.StartLevelAnim();
    }

    public void EndLevel()
    {
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

    private void SubscribeEvents()
    {
        gameEventHandler.OnLevelStarted += StartLevel;
        gameEventHandler.OnLevelEnd += EndLevel;
        gameEventHandler.OnLevelRestart += RestartLevel;
        gameEventHandler.OnLevelWin += WinLevel;
        gameEventHandler.OnLevelFailed += FailLevel;
    }
}
