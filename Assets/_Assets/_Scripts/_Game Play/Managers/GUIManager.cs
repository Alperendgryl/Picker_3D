using DG.Tweening;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private void Update()
    {
        LevelIndicator();
    }

    [SerializeField] private TMP_Text diamondTXT;
    [SerializeField] private DOTweenAnimation[] uiElements;
    public void StartLevelAnim()
    {
        foreach (DOTweenAnimation animation in uiElements)
        {
            animation.DOPlayAllById("0");
        }
    }

    private void UpdateDiamond()
    {
        diamondTXT.text = "5";
    }

    [SerializeField] GameObject[] LevelIndicatorObjects;
    private void LevelIndicator()
    {
        for (int i = 0; i < LevelIndicatorObjects.Length; i++)
        {
            switch (LevelIndicatorObjects[i].gameObject.name)
            {
                case "CurrentLevel":
                    break;
                case "NextLevel":
                    break;
                case "Stage1":
                    break;
                case "Stage2":
                    break;
                case "Stage3":
                    break;
                default: break;
            }
        }
    }

    public void LevelFailedOrWin()
    {

    }
}
