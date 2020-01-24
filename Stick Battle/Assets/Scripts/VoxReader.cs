using UnityEngine;
using System.IO;

public class VoxReader : MonoBehaviour
{
    public string FileName;

    public Map Map;

    private void Start()
    {
        LoadMapData();
    }

    private void LoadMapData()
    {
        string path = $"{Application.dataPath}/Objects/Maps/{FileName}.vox";

        if (File.Exists(path))
        {
            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                br.ReadChars(4); //VOX_
                br.ReadInt32(); //Version
                br.ReadChars(4); //MAIN
                br.ReadInt32(); //ChunkContentSize
                br.ReadInt32(); //ChildrenChunks
                br.ReadChars(4); //SIZE
                br.ReadInt32(); //ChunkContentSize
                br.ReadInt32(); //ChildrenChunks

                int mapSizeX = br.ReadInt32(),
                    mapSizeZ = br.ReadInt32(),
                    mapSizeY = br.ReadInt32();

                Map.MapSize = new Vector3(mapSizeX, mapSizeY, mapSizeZ);
                Map.MapData = new byte[mapSizeX, mapSizeZ, mapSizeZ];

                br.ReadChars(4); //XYZI
                br.ReadInt32(); //ChunkContentSize
                br.ReadInt32(); //ChildrenChunks

                int numVoxels = br.ReadInt32();
                
                for(int n = 0; n < numVoxels; n++)
                {
                    byte x = br.ReadByte(),
                         z = br.ReadByte(),
                         y = br.ReadByte(),
                         i = br.ReadByte();

                    Map.MapData[x, y, z] = i;
                }
            }
        }
    }
    /*
    private void SerializeMapData()
    {
        using(BinaryWriter bw = new BinaryWriter(File.Open($"{Application.dataPath}/Objects/Maps/{NewFileName}.map", FileMode.Create)))
        {
            Debug.Log("SERIALIZATION HAS BEEN STARTED");

            bw.Write(MapData.GetLength(0)); //MapSizeX
            bw.Write(MapData.GetLength(1)); //MapSizeY
            bw.Write(MapData.GetLength(2)); //MapSizeZ

            for (int x = 0; x < MapData.GetLength(0); x++)
                for (int y = 0; y < MapData.GetLength(1); y++)
                    for (int z = 0; z < MapData.GetLength(2); z++)
                        bw.Write($"{MapData[x, y, z]}");

            Debug.Log("SERIALIZATION HAS BEEN DONE");
        }
    }
    */
}
