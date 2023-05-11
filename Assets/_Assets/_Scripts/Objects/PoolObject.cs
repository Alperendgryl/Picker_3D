using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private TMP_Text ccText;
    public int ccValue;

    #region Level Editor
    private int MIN_CC_VALUE = 5;

    private void Start()
    {
        collectedValue = 0;
        UpdateCCText();
    }

    public void IncrementPoolValue()
    {
        ccValue++;
        UpdateCCText();
    }

    public void DecrementPoolValue()
    {
        if (ccValue > MIN_CC_VALUE)
        {
            ccValue--;
            UpdateCCText();
        }
    }

    private void UpdateCCText()
    {
        ccText.text = $"0/{ccValue}";
    }
    #endregion

    #region GamePlay

    [SerializeField] private GameObject poolInside;
    [SerializeField] private GameObject poolGate;

    private int collectedValue;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectedValue++;
            UpdateGPCCValue();
            CheckPoolStatus();
        }
    }

    private void UpdateGPCCValue()
    {
        ccText.text = $"{collectedValue}/{ccValue}";
    }

    private void CheckPoolStatus()
    {
        if (collectedValue > ccValue)
        {
            StartCoroutine(PoolAnimations());
        }
        else
        {
            Debug.Log(collectedValue);
        }
    }

    private IEnumerator PoolAnimations()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < poolGate.transform.childCount; i++) // Animate the pool gates
        {
            poolGate.transform.GetChild(i).gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        }

        poolInside.gameObject.transform.DOMoveY(0, 1.5f); // Move the pool to the surface

        yield return new WaitForSeconds(1.5f); // Wait for the animations to finish

        FindObjectOfType<GameManager>().GameEventHandler.TriggerPoolAnimationsFinished(); // Trigger the event
    }
    #endregion
}
