using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    public const int ChunkSize = 16;

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();
    private List<Vector2> newUV2 = new List<Vector2>();

    private float tUnit1 = 1/256f;
    private float tUnit2 = 1/4f;

    private int faceCount;

    private float colorPos1;
    private float colorPos2;
    
    public void GenerateMesh(int _x, int _y, int _z)
    {
        for (int x_ = 0; x_ < ChunkSize; x_++)
            for (int y_ = 0; y_ < ChunkSize; y_++)
                for (int z_ = 0; z_ < ChunkSize; z_++)
                {
                    int x = _x * ChunkSize + x_,
                        y = _y * ChunkSize + y_,
                        z = _z * ChunkSize + z_;

                    if (World.WorldData[x, y, z] != 0)
                    {
                        colorPos1 = (World.WorldData[x, y, z] - 1) * tUnit1;
                        colorPos2 = World.HealthData[x, y, z] * tUnit2;

                        if (z != World.WorldData.GetLength(2) - 1 && World.WorldData[x, y, z + 1] == 0)
                            FaceNorth(x_, y_, z_);
                        if (x != World.WorldData.GetLength(0) - 1 && World.WorldData[x + 1, y, z] == 0)
                            FaceEast(x_, y_, z_);
                        if (z != 0 && World.WorldData[x, y, z - 1] == 0)
                            FaceSouth(x_, y_, z_);
                        if (x != 0 && World.WorldData[x - 1, y, z] == 0)
                            FaceWest(x_, y_, z_);
                        if (y == World.WorldData.GetLength(1) - 1 || World.WorldData[x, y + 1, z] == 0)
                            FaceTop(x_, y_, z_);
                        if (y != 0 && World.WorldData[x, y - 1, z] == 0)
                            FaceBottom(x_, y_, z_);
                    }
                }

        UpdateMesh();
    }

    private void FaceNorth(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y + 1, z + 1));
        newVertices.Add(new Vector3(x, y + 1, z + 1));
        SetFace();
    }
    private void FaceEast(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y + 1, z));
        newVertices.Add(new Vector3(x + 1, y + 1, z + 1));
        SetFace();
    }
    private void FaceSouth(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y + 1, z));
        newVertices.Add(new Vector3(x + 1, y + 1, z));
        SetFace();
    }
    private void FaceWest(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y + 1, z + 1));
        newVertices.Add(new Vector3(x, y + 1, z));
        SetFace();
    }
    private void FaceTop(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x, y + 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y + 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y + 1, z));
        newVertices.Add(new Vector3(x, y + 1, z));
        SetFace();
    }
    private void FaceBottom(int x, int y, int z)
    {
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        SetFace();
    }

    private void SetFace()
    {
        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 1);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4 + 3);

        newUV.Add(new Vector2(colorPos1, 1));
        newUV.Add(new Vector2(colorPos1 + tUnit1, 1));
        newUV.Add(new Vector2(colorPos1 + tUnit1, 0));
        newUV.Add(new Vector2(colorPos1, 0));   

        newUV2.Add(new Vector2(0, colorPos2));
        newUV2.Add(new Vector2(1, colorPos2));
        newUV2.Add(new Vector2(1, colorPos2 + tUnit2));
        newUV2.Add(new Vector2(0, colorPos2 + tUnit2));    

        faceCount++;
    }

    private void UpdateMesh()
    {
        DestroyImmediate(GetComponent<MeshCollider>());

        GetComponent<MeshFilter>().mesh = new Mesh();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.uv2 = newUV2.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.Optimize();

        gameObject.AddComponent(typeof(MeshCollider));

        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
        newUV2.Clear();

        faceCount = 0;
    }
}
