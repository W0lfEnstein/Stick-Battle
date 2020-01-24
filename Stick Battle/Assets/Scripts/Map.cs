using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    public string MapId;
    public string MapName;

    public Vector3 MapSize;

    public byte[,,] MapData;

    public void LoadMapData()
    {
        string path = $"{Application.dataPath}/Objects/Maps/{MapId}.vox";

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

                MapSize = new Vector3(mapSizeX, mapSizeY, mapSizeZ);
                MapData = new byte[mapSizeX, mapSizeZ, mapSizeZ];

                br.ReadChars(4); //XYZI
                br.ReadInt32(); //ChunkContentSize
                br.ReadInt32(); //ChildrenChunks

                int numVoxels = br.ReadInt32();

                for (int n = 0; n < numVoxels; n++)
                {
                    byte x = br.ReadByte(),
                         z = br.ReadByte(),
                         y = br.ReadByte(),
                         i = br.ReadByte();

                    MapData[x, y, z] = i;
                }
            }
        }
    }
}