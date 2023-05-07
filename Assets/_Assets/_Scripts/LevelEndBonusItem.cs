using TMPro;
using UnityEngine;

public class LevelEndBonusItem : MonoBehaviour
{
    public int _itemNumber = 0;
    public GameObject BonusNumText;

    private void Awake()
    {
        int numMultiplier = Random.Range(1, 101);
        _itemNumber = numMultiplier * 10;

        BonusNumText = transform.Find("BonusNumber").gameObject;
        BonusNumText.GetComponent<TMP_Text>().text = _itemNumber.ToString();
    }
}
