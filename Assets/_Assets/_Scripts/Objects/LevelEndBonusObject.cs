using TMPro;
using UnityEngine;

public class LevelEndBonusObject : MonoBehaviour
{
    public int diamondValue = 0;
    private TMP_Text diamondValueTXT;

    private void Awake()
    {
        int numMultiplier = Random.Range(1, 101);
        diamondValue = numMultiplier * 10;

        // Access the grandchild
        diamondValueTXT = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        if (diamondValueTXT != null)
        {
            diamondValueTXT.text = diamondValue.ToString(); //Top of the bonus object 3D text.
        }
        else
        {
            Debug.LogError("Grandchild does not have a TMP_Text component.");
        }
    }
}
