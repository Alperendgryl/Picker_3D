using UnityEngine;

public class GameManager : MonoBehaviour, IGameLevelHandler
{
    private GUIManager guiManager;
    private PickerController pickerController;
    private GameEventHandler gameEventHandler;
    private LevelInitializer levelsInitializers;
    private PoolManager poolManager;
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

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameEventHandler = new GameEventHandler();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initializer();
        SubscribeEvents();
    }

    private void Initializer()
    {
        guiManager = FindObjectOfType<GUIManager>();
        pickerController = FindObjectOfType<PickerController>();
        levelsInitializers = FindObjectOfType<LevelInitializer>();
        poolManager = FindObjectOfType<PoolManager>();
        guiManager.UpdateLevelIndicator();
    }

    public void StartLevel()
    {
        pickerController.MovePicker();
        guiManager.HideButtonsAnim(1.25f);
    }

    public void WinLevel() //0 = Win, 1 = Fail, 2 = Level Panel (Indicators)
    {
        pickerController.StopPicker();

        guiManager.PanelController(0, true); // Win Panel Active
    }

    public void FailLevel() //0 = Win, 1 = Fail, 2 = Level Panel (Indicators)
    {
        pickerController.StopPicker();

        guiManager.PanelController(1, true); // Fail Panel Active
    }

    public void NextLevel()
    {
        levelsInitializers.LoadNextLevel();

        guiManager.PanelController(2, true); // Level Indicators Active

        pickerController.RestartPickerPos();

        guiManager.UpdateLevelIndicator();

        guiManager.ShowButtonsAnim(1.25f);

        poolManager.ResetPoolPassed();

        guiManager.ChangePoolStageColor(0);
    }

    public void RestartLevel()
    {
        levelsInitializers.LoadLevelAgain();

        guiManager.PanelController(2, true); // Level Indicators Active

        pickerController.RestartPickerPos();

        guiManager.ShowButtonsAnim(1.25f);

        poolManager.ResetPoolPassed();

        guiManager.ChangePoolStageColor(0);
    }

    private void SubscribeEvents()
    {
        gameEventHandler.OnLevelStarted += StartLevel;
        gameEventHandler.OnLevelWin += WinLevel;
        gameEventHandler.OnLevelFailed += FailLevel;
    }

    private void OnDestroy()
    {
        gameEventHandler.OnLevelStarted -= StartLevel;
        gameEventHandler.OnLevelWin -= WinLevel;
        gameEventHandler.OnLevelFailed -= FailLevel;
    }
}