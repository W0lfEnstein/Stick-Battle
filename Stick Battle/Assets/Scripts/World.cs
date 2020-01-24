using UnityEngine;

public class World : MonoBehaviour
{
    public static World instance;

    public static byte[,,] WorldData;
    public static byte[,,] HealthData;
    public static Chunk[,,] Chunks;

    public Chunk Chunk;

    private Map map;

    private void Awake()
    {
        if (!instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        map = GetComponent<Map>();
        map.LoadMapData();

        int worldSizeX = (int)map.MapSize.x,
            worldSizeY = (int)map.MapSize.x,
            worldSizeZ = (int)map.MapSize.x;

        WorldData = map.MapData;
        HealthData = new byte[worldSizeX, worldSizeY, worldSizeZ];

        Chunks = new Chunk[worldSizeX / Chunk.ChunkSize, worldSizeY / Chunk.ChunkSize, worldSizeZ / Chunk.ChunkSize];

        CalculateHealthData();
        CreateWorld();
    }  

    #region World Generation Settings

    private void CalculateHealthData()
    {
        for (int x = 0; x < HealthData.GetLength(0); x++)
            for (int y = 0; y < HealthData.GetLength(1); y++)
                for (int z = 0; z < HealthData.GetLength(2); z++)
                {
                    HealthData[x, y, z] = 3;
                }
    }

    private void CreateWorld()
    {
        for (int x = 0; x < Chunks.GetLength(0); x++)
            for (int y = 0; y < Chunks.GetLength(1); y++)
                for (int z = 0; z < Chunks.GetLength(2); z++)
                {
                    Chunk newChunk = Instantiate(Chunk, new Vector3(x * Chunk.ChunkSize, y * Chunk.ChunkSize, z * Chunk.ChunkSize), Quaternion.identity);
                    newChunk.transform.parent = transform;

                    Chunks[x, y, z] = newChunk;

                    newChunk.GenerateMesh(x, y, z);
                }
    }

    #endregion

    #region Chunk Settings

    public void DamageBlockAt(Vector3 pos, int damage)
    {
        int x = (int)pos.x,
            y = (int)pos.y,
            z = (int)pos.z;

        HealthData[x, y, z] -= (byte) damage;

        if (HealthData[x, y, z] == 0)
        {
            HealthData[x, y, z] = 3;

            RemoveBlockAt(pos);
        }
        else
            UpdateChunkAt(pos);
    }
    public void SetBlockAt(Vector3 pos, byte block)
    {
        int x = (int)pos.x,
            y = (int)pos.y,
            z = (int)pos.z;

        WorldData[x, y, z] = block;

        UpdateChunkAt(pos);

        if ((x + 1) % 16 == 0)
            UpdateChunkAt(new Vector3(pos.x + 1, pos.y, pos.z));
        if((x != 0 && x % 16 == 0))
            UpdateChunkAt(new Vector3(pos.x - 1, pos.y, pos.z));
        if((y + 1) % 16 == 0)
            UpdateChunkAt(new Vector3(pos.x, pos.y + 1, pos.z));
        if (y != 0 && y % 16 == 0)
            UpdateChunkAt(new Vector3(pos.x, pos.y - 1, pos.z));
        if ((z + 1) % 16 == 0)
            UpdateChunkAt(new Vector3(pos.x, pos.y, pos.z + 1));
        if (z != 0 && z % 16 == 0)
            UpdateChunkAt(new Vector3(pos.x, pos.y, pos.z - 1));
    }
    public void RemoveBlockAt(Vector3 pos)
    {
        SetBlockAt(pos, 0);
    }

    private void UpdateChunkAt(Vector3 pos)
    {
        int x = (int)pos.x / Chunk.ChunkSize,
            y = (int)pos.y / Chunk.ChunkSize,
            z = (int)pos.z / Chunk.ChunkSize;

        Chunks[x, y, z].GenerateMesh(x, y, z);
    }
    private void UpdateChunkAll()
    {
        for (int x = 0; x < Chunks.GetLength(0); x++)
            for (int y = 0; y < Chunks.GetLength(1); y++)
                for (int z = 0; z < Chunks.GetLength(2); z++)
                    Chunks[x, y, z].GenerateMesh(x, y, z);
    }

    #endregion
}
