using UnityEngine;

public class Noise : MonoBehaviour
{
    public static Noise instance;

    public float Length;
    public float Width;
    public float Depth;
    public float Scale;

    public float OffsetX;
    public float OffsetY;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else Destroy(gameObject);
    }

    public int[,] GenerateHeights(int xSize, int ySize)
    {
        int[,] heightMap = new int[xSize, ySize];

        for (int x = 0; x < heightMap.GetLength(0); x++)
            for (int y = 0; y < heightMap.GetLength(1); y++)
                heightMap[x, y] = CalculateHeight(x, y);

        return heightMap;
    }

    private int CalculateHeight(int x, int y)
    {
        float xCoord = (x + 1 + OffsetX) / Length * Scale,
              yCoord = (y + 1 + OffsetY) / Width * Scale;

        float noise = Mathf.PerlinNoise(xCoord, yCoord);

        return (int)(noise * Depth);
    }
}
