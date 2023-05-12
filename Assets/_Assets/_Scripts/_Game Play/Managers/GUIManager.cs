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

    public void HideButtonsAnim(float animDuration)
    {
        StartTheAnim(0, 0, -150, animDuration);
        StartTheAnim(1, -250, 0, animDuration);
        StartTheAnim(2, 250, 0, animDuration);
        StartTheAnim(3, 250, -250, animDuration);
    }

    public void ShowButtonsAnim(float animDuration)
    {
        StartTheAnim(0, 0, 150, animDuration);
        StartTheAnim(1, 25, 0, animDuration);
        StartTheAnim(2, -25, 0, animDuration);
        StartTheAnim(3, -25, -250, animDuration);
    }

    private void StartTheAnim(int elementID, float xPos, float yPos, float animationDuration)
    {
        StartScreenUIElements[elementID].GetComponent<RectTransform>().DOAnchorPos(new Vector2(xPos, yPos), animationDuration);
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
    public void SetPoolStageColorsToInitials()
    {
        for (int i = 0; i < poolStages.Length; i++)
        {
            poolStages[i].GetComponent<Image>().color = Color.white;
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