using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public Texture2D map;
    public ColorToPrefab[] colorMappings;
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int width = 0; width < map.width; width++)
        {
            for (int height = 0; height < map.height; height++)
            {
                GenerateTile(width, height);
            }
        }
    }

    void GenerateTile(int width, int height)
    {
        Color pixelColor = map.GetPixel(width, height);

        if (pixelColor.a == 0) return; // Ignore each transparrent pixel.

        foreach (ColorToPrefab colorMapping in colorMappings)
        {

            if (colorMapping.color.Equals(pixelColor))
            {
                Vector2 pos = new Vector2(width, height); // Vector3 pos = new Vector3(width, 0, height); //horizontally
                Instantiate(colorMapping.prefab, pos, Quaternion.identity, transform);
            }
        }
    }

    //https://www.youtube.com/watch?v=B_Xp9pt8nRY
}
