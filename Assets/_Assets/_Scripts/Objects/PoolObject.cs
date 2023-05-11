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
    private bool isRunningPoolAnimations = false;
    public int poolPassed = 0;

    private AudioManager audioManager;
    private GUIManager guiManager;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        guiManager = FindObjectOfType<GUIManager>();
        poolInside = transform.Find("Pool Inside").gameObject;
        //poolGate = transform.Find("Pool Gate").gameObject;
        poolGate = GameObject.FindGameObjectWithTag("PoolGate");
    }

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
            audioManager.PlayCollectableSFX();
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
        if (collectedValue >= ccValue && !isRunningPoolAnimations)
        {
            StartCoroutine(PoolAnimations());
        }
        else
        {
            StartCoroutine(DelayedTriggerLevelFailed(3f));
        }
    }

    private IEnumerator DelayedTriggerLevelFailed(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collectedValue < ccValue && !isRunningPoolAnimations)
        {
            GameManager.Instance.GameEventHandler.TriggerLevelFailed();
            poolPassed = 0;
        }
    }

    private IEnumerator PoolAnimations()
    {
        isRunningPoolAnimations = true;

        yield return new WaitForSeconds(2f);
        AnimatePoolGates();
        MovePoolToSurface();

        yield return new WaitForSeconds(1.5f); // Wait for the animations to finish

        GameObject stageAreaChild = FindChildWithTag(transform, "StageArea");
        if (stageAreaChild != null)
        {
            stageAreaChild.SetActive(false);
        }

        FindObjectOfType<PickerController>().MovePicker();

        isRunningPoolAnimations = false; // signal that the animations have finished
        PoolManager.Instance.poolPassed++; // Increment the shared poolPassed value
        guiManager.ChangePoolStageColor(PoolManager.Instance.poolPassed); // Use the shared poolPassed value
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
        audioManager.PlayPoolGateSFX();
    }

    GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            var result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result.gameObject;
            }
        }

        return null;
    }
    #endregion
}