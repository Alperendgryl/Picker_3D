using TMPro;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private TMP_Text ccText;
    public int ccValue;
    public int minCCValue = 5;
    public int initialCCValue = 10;

    private void Start()
    {
        ccValue = initialCCValue;
        UpdateCCText();
    }

    public void IncrementPoolValue()
    {
        ccValue++;
        UpdateCCText();
    }

    public void DecrementPoolValue()
    {
        if (ccValue > minCCValue)
        {
            ccValue--;
            UpdateCCText();
        }
    }

    private void UpdateCCText()
    {
        ccText.text = $"0/{ccValue}";
    }
}
