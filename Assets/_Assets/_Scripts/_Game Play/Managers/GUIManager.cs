using DG.Tweening;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
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

    private void LevelIndicator()
    {

    }
}
