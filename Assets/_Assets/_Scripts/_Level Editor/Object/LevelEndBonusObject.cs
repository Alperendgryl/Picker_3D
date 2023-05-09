using TMPro;
using UnityEngine;

public class LevelEndBonusObject : MonoBehaviour
{
    [SerializeField] private int _itemNumber = 0;
    [SerializeField] private GameObject BonusNumText;

    private void Awake()
    {
        int numMultiplier = Random.Range(1, 101);
        _itemNumber = numMultiplier * 10;

        BonusNumText.GetComponent<TMP_Text>().text = _itemNumber.ToString();
    }
}
