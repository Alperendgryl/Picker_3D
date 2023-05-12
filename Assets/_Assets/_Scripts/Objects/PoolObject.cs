using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    // Fields related to Level Editor
    [Header("Level Editor")]
    [SerializeField] private TMP_Text ccText;
    [SerializeField] private int MIN_CC_VALUE = 5;
    public int ccValue;

    // Fields related to GamePlay
    [Header("GamePlay")]
    [SerializeField] private GameObject poolInside;
    [SerializeField] private GameObject poolGate;
    private int collectedValue;
    private bool isRunningPoolAnimations = false;
    public int poolPassed = 0;

    // Other dependencies
    private AudioManager audioManager;
    private GUIManager guiManager;

    private void Awake()
    {
        // Find and assign dependencies
        audioManager = FindObjectOfType<AudioManager>();
        guiManager = FindObjectOfType<GUIManager>();
        poolInside = transform.Find("Pool Inside").gameObject;
        poolGate = transform.Find("Pool Gate").gameObject;
    }

    private void Start()
    {
        // Initialize the pool
        InitializePool();
    }

    #region Level Editor
    // Functions related to Level Editor

    // Initialize the pool with the given values
    private void InitializePool()
    {
        collectedValue = 0;
        UpdateCCText();
    }

    // Increase the pool value by one
    public void IncrementPoolValue()
    {
        ccValue++;
        UpdateCCText();
    }

    // Decrease the pool value by one if it's greater than MIN_CC_VALUE
    public void DecrementPoolValue()
    {
        if (ccValue > MIN_CC_VALUE)
        {
            ccValue--;
            UpdateCCText();
        }
    }

    // Update the text on the ccText object
    private void UpdateCCText()
    {
        ccText.text = $"0/{ccValue}";
    }
    #endregion

    #region GamePlay
    // Functions related to GamePlay

    // Collect item if collision with collectable
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            CollectItem();
            audioManager.PlayCollectableSFX();
        }
    }

    // Update collected value and check the pool status
    private void CollectItem()
    {
        collectedValue++;
        UpdateGPCCValue();
        CheckPoolStatus();
    }

    // Update the text on the ccText object for gameplay
    private void UpdateGPCCValue()
    {
        ccText.text = $"{collectedValue}/{ccValue}";
    }

    // Check the pool status and trigger animations or failure accordingly
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

    // Trigger Level Failed after a delay if conditions are met
    private IEnumerator DelayedTriggerLevelFailed(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collectedValue < ccValue && !isRunningPoolAnimations)
        {
            GameManager.Instance.GameEventHandler.TriggerLevelFailed();
            poolPassed = 0;
        }
    }

    // Run pool animations and related operations
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

        isRunningPoolAnimations = false; // Signal that the animations have finished
        PoolManager.Instance.poolPassed++; // Increment the shared poolPassed value
        guiManager.ChangePoolStageColor(PoolManager.Instance.poolPassed); // Use the shared poolPassed value
    }

    // Animate the pool gates
    private void AnimatePoolGates()
    {
        for (int i = 0; i < poolGate.transform.childCount; i++) // Animate the pool gates
        {
            poolGate.transform.GetChild(i).gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        }
    }

    // Move the pool to the surface
    private void MovePoolToSurface()
    {
        poolInside.gameObject.transform.DOMoveY(0, 1.5f); // Move the pool to the surface
        audioManager.PlayPoolGateSFX();
    }

    // Find a child with a specific tag within the children of the given transform
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

