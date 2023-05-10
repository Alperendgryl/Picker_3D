using DG.Tweening;
using UnityEngine;

public class UIAnimationManager : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation[] uiElements;

    public void StartLevelAnim()
    {
        foreach (DOTweenAnimation animation in uiElements)
        {
            animation.DOPlayAllById("0");
        }
    }

    public void EndLevelAnim()
    {
        foreach (DOTweenAnimation animation in uiElements)
        {
            animation.DOPlayAllById("1");
        }
    }

    public void WinLevelAnim()
    {
        foreach (DOTweenAnimation animation in uiElements)
        {
            animation.DOPlayAllById("2");
        }
    }

    public void FailLevelAnim()
    {
        foreach (DOTweenAnimation animation in uiElements)
        {
            animation.DOPlayAllById("3");
        }
    }

    public void RestartLevelAnim()
    {
        foreach (DOTweenAnimation animation in uiElements)
        {
            animation.DOPlayAllById("4");
        }
    }
}
