using DG.Tweening;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation[] uiElements;
    public void StartLevelAnim()
    {
        foreach (DOTweenAnimation animation in uiElements)
        {
            animation.DOPlayAllById("0");
        }
    }

    #region Start Panel
    #endregion

    #region End Panel
    #endregion

    #region Shop Panel
    #endregion

    #region Missions Panel
    #endregion

    #region Challenge Panel
    #endregion

    #region Diamond
    #endregion

    #region Level Indicator
    #endregion
}
