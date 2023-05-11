using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private Color pickerColor;
    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject[] picker;

    private Dictionary<int, bool> UnlockedPickers;
    private void Start()
    {
        UnlockedPickers = new Dictionary<int, bool>();
    }

    public void SetPickerColor()
    {
        int pickerID = transform.GetSiblingIndex();
        pickerColor = colors[pickerID];
        FindObjectOfType<PickerController>().ChangePickerColor();
    }


    public void UnlockRandom()
    {
        //if(Diamond > 3000)
        int random = Random.Range(1, picker.Length);

        if (UnlockedPickers.ContainsKey(random)) UnlockRandom();

        picker[random].transform.GetChild(0).gameObject.SetActive(true);
        picker[random].transform.GetChild(1).gameObject.SetActive(false);
        UnlockedPickers.Add(random, true);
    }

    public Color GetPickerColor()
    {
        return pickerColor;
    }
}
