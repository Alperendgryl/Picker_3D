using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    LevelDataHandler dataHandler;

    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject[] environments;

    private const int LEVEL_COUNT = 11;
    private int playerLevel; // The player's progression in the game (This will be shown as indicator)
    private int levelArrayIndex; // The index of the level in the array to play (Levels continues to infinity in a random order)
    private void Awake()
    {
        dataHandler = LevelDataHandler.Instance;
        dataHandler.LoadGameData();
    }
    private void Start()
    {
        playerLevel = dataHandler.playerLevel;
        levelArrayIndex = dataHandler.levelArrayIndex;

        Instantiate(environments[Random.Range(0, environments.Length)]);
        Instantiate(levels[levelArrayIndex]);
    }

    public void LoadNextLevel()
    {
        playerLevel++;

        Destroy(GameObject.FindGameObjectWithTag("Plane"));
        Instantiate(environments[Random.Range(0, environments.Length)]);

        Destroy(GameObject.FindGameObjectWithTag("Level"));
        if (playerLevel < LEVEL_COUNT)
        {
            levelArrayIndex = playerLevel;
        }
        else
        {
            levelArrayIndex = Random.Range(0, LEVEL_COUNT);
        }
        Instantiate(levels[levelArrayIndex]);

        dataHandler.playerLevel = playerLevel;
        dataHandler.levelArrayIndex = levelArrayIndex;
        dataHandler.SaveGameData();
    }

    public void LoadLevelAgain()
    {
        Destroy(GameObject.FindGameObjectWithTag("Level"));
        Instantiate(levels[levelArrayIndex]);
    }
}