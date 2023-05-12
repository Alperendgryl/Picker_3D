using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text diamondTXT, currentLevel, nextLevel;

    [SerializeField] GameObject InLevelPanel, WinPanel, FailPanel;

    [SerializeField] GameObject[] poolStages, StartScreenUIElements;

    [SerializeField] private Button nextLevelButton, replayLevelButton;

    [SerializeField] private Color passedPoolColor = Color.green;

    private LevelDataHandler dataHandler;
    private void Awake()
    {
        dataHandler = LevelDataHandler.Instance;
    }
    private void Start()
    {
        UpdateDiamondText();

        nextLevelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.NextLevel();
            PanelController(0, false); //Close Win Panel
        });

        replayLevelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartLevel();
            PanelController(1, false); //Close Fail Panel
        });

    }

    public void HideButtonsAnim(float animationDuration)
    {
        // Move element at index 0 from Y 150 to Y -150
        StartScreenUIElements[0].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -150), animationDuration);

        // Move element at index 1 from X 25 to X -250
        StartScreenUIElements[1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-250, 0), animationDuration);

        // Move element at index 2 from X -25 to X 250
        StartScreenUIElements[2].GetComponent<RectTransform>().DOAnchorPos(new Vector2(250, 0), animationDuration);

        // Move element at index 3 from X -25, Y -250 to X 250, Y -250
        StartScreenUIElements[3].GetComponent<RectTransform>().DOAnchorPos(new Vector2(250, -250), animationDuration);
    }

    public void ShowButtonsAnim(float animationDuration)
    {
        // Move element at index 0 from Y 150 to Y -150
        StartScreenUIElements[0].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 150), animationDuration);

        // Move element at index 1 from X 25 to X -250
        StartScreenUIElements[1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(25, 0), animationDuration);

        // Move element at index 2 from X -25 to X 250
        StartScreenUIElements[2].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-25, 0), animationDuration);

        // Move element at index 3 from X -25, Y -250 to X 250, Y -250
        StartScreenUIElements[3].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-25, -250), animationDuration);
    }

    public void UpdateLevelIndicator()
    {
        int level = dataHandler.playerLevel;
        currentLevel.text = (level + 1).ToString();
        nextLevel.text = (level + 2).ToString();
    }

    public void UpdateDiamondText()
    {
        diamondTXT.text = dataHandler.diamond.ToString();
    }

    public void ChangePoolStageColor(int passedPools)
    {
        if (passedPools <= poolStages.Length)
        {
            Image poolStageImage = poolStages[passedPools - 1].GetComponent<Image>();
            if (poolStageImage != null)
            {
                poolStageImage.color = passedPoolColor;
            }
        }
    }

    public void PanelController(int PanelID, bool state)
    {
        switch (PanelID)
        {
            case 0:
                WinPanel.SetActive(state);
                break;
            case 1:
                FailPanel.SetActive(state);
                break;
            case 2:
                InLevelPanel.SetActive(state);
                break;
        }
    }
}