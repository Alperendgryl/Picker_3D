using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [Header("Level Editor")]
    [SerializeField] private TMP_Text ccText;
    [SerializeField] private int MIN_CC_VALUE = 5;
    public int ccValue;

    [Header("GamePlay")]
    [SerializeField] private GameObject poolInside;
    [SerializeField] private GameObject poolGate;

    private int collectedValue;

    private void Start()
    {
        InitializePool();
    }

    #region Level Editor
    private void InitializePool()
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
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        collectedValue++;
        UpdateGPCCValue();
        CheckPoolStatus();
    }

    private void UpdateGPCCValue()
    {
        ccText.text = $"{collectedValue}/{ccValue}";
    }

    private void CheckPoolStatus()
    {
        if (collectedValue >= ccValue)
        {
            StartCoroutine(PoolAnimations());
        }
        else
        {
            // Level Failed, Open fail panel
        }
    }

    private IEnumerator PoolAnimations()
    {
        yield return new WaitForSeconds(2f);
        AnimatePoolGates();
        MovePoolToSurface();

        yield return new WaitForSeconds(1.5f); // Wait for the animations to finish
        FindObjectOfType<GameManager>().GameEventHandler.TriggerPoolAnimationsFinished(); // Trigger the event
    }

    private void AnimatePoolGates()
    {
        for (int i = 0; i < poolGate.transform.childCount; i++) // Animate the pool gates
        {
            poolGate.transform.GetChild(i).gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        }
    }

    private void MovePoolToSurface()
    {
        poolInside.gameObject.transform.DOMoveY(0, 1.5f); // Move the pool to the surface
    }
    #endregion
}
