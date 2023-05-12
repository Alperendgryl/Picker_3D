using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private Color pickerColor;
    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject[] picker;

    public Dictionary<int, bool> UnlockedPickers;
    private LevelDataHandler dataHandler;

    const int PICKER_SKIN_COST = 3000;

    public static Store Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dataHandler = LevelDataHandler.Instance;
        UnlockedPickers = new Dictionary<int, bool>();
    }
    private void Start()
    {
        UnlockedPickers = new Dictionary<int, bool>();
    }

    public void UnlockPicker(int pickerID)
    {
        picker[pickerID].transform.GetChild(0).gameObject.SetActive(true);
        picker[pickerID].transform.GetChild(1).gameObject.SetActive(false);
        UnlockedPickers.Add(pickerID, true);
    }

    public void SetPickerColor(int pickerID)
    {
        // Ensure pickerID is within bounds of the colors array
        if (pickerID < 0 || pickerID >= colors.Length)
        {
            Debug.LogError("Invalid pickerID: " + pickerID);
            return;
        }

        // Set the picker color
        pickerColor = colors[pickerID];

        // Change the color of the picker in the game
        FindObjectOfType<PickerController>().ChangePickerColor(pickerColor);
    }

    public void UnlockRandom()
    {
        if (dataHandler.diamond >= PICKER_SKIN_COST)
        {
            int random = Random.Range(1, picker.Length);

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
}
