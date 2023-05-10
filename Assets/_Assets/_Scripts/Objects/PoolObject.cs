using TMPro;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private TMP_Text ccText;
    public int ccValue;
    public int MIN_CC_VALUE = 5;

    private int collectedValue;

    #region Level Editor
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
    private void Update()
    {
        UpdateGPCCValue();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectedValue++;
            UpdateGPCCValue();
        }
    }

    private void UpdateGPCCValue()
    {
        ccText.text = $"{collectedValue}/{ccValue}";
    }
    #endregion
}
