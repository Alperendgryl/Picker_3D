using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private Color pickerColor;
    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject[] picker;

    private Dictionary<int, bool> UnlockedPickers;
    private LevelDataHandler dataHandler;

    const int PICKER_SKIN_COST = 3000;
    private void Awake()
    {
        dataHandler = LevelDataHandler.Instance;
    }
    private void Start()
    {
        UnlockedPickers = new Dictionary<int, bool>();
    }

    public void SetPickerColor()
    {
        //Attached to each button onClick. The ID of each button is equal to child order if button is 2th child the id is 2. When the user clicks a button change the color of the picker according to it
        int pickerID = transform.GetSiblingIndex(); 
        pickerColor = colors[pickerID];
        FindObjectOfType<PickerController>().ChangePickerColor();
    }


    public void UnlockRandom()
    {
        if (dataHandler.diamond >= PICKER_SKIN_COST)
        {
            int random = Random.Range(1, picker.Length); //select a random gameobject and add it to list according to its child id (if 3th child add to a list as 3th child is unlocked)

            if (UnlockedPickers.ContainsKey(random))
            {
                UnlockRandom();
                return;
            }

            picker[random].transform.GetChild(0).gameObject.SetActive(true);
            picker[random].transform.GetChild(1).gameObject.SetActive(false);
            UnlockedPickers.Add(random, true);


            dataHandler.UpdateDiamond(-PICKER_SKIN_COST); // Assuming LevelDataHandler has a SaveDiamond method.

            FindObjectOfType<GUIManager>().UpdateDiamondText(); // Assuming GUIManager has an UpdateDiamondText method.
        }
    }

    public Color GetPickerColor()
    {
        return pickerColor;
    }
}
