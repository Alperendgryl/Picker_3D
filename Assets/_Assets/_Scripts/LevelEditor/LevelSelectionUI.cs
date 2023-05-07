using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform levelButtonContainer;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        int numberOfLevels = levelManager.GetNumberOfLevels();

        for (int i = 1; i <= numberOfLevels; i++)
        {
            GameObject button = Instantiate(levelButtonPrefab, levelButtonContainer);
            button.GetComponentInChildren<Text>().text = "Level " + i;
            int levelNumber = i;
            button.GetComponent<Button>().onClick.AddListener(() => levelManager.SelectLevel(levelNumber));
        }
    }
}
