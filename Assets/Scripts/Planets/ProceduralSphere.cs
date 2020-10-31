using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSphere : MeshGenerator
{
    [SerializeField]
    private float size;
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    [SerializeField]
    private int chunkSize = 2;

    [SerializeField]
    private int chunksPerFace;

    private Chunk[] chunks;

    private int nrOfSections;

    void Awake()
    {
        chunkSize = 2;
        mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>().mesh = mesh;

        chunks = new Chunk[chunksPerFace * chunksPerFace * 8];

        nrOfSections = chunksPerFace * chunkSize;

        CreateOctaeder();
        Bloat();
        Section();
        Bloat();
    }

    private void CreateOctaeder()
    {
        vertices = new Vector3[] { Vector3.up, Vector3.forward, Vector3.right, Vector3.back, Vector3.left, Vector3.down };
        triangles = new int[]{
            2,0,1,
            1,0,4,
            3,0,2,
            4,0,3,
            1,5,2,
            4,5,1,
            2,5,3,
            3,5,4,
        };
    }

    private void Bloat()
    {
        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v] *= size / vertices[v].magnitude;
        }
    }

    private void Section()
    {
        List<Vector3Int> newTriangles = new List<Vector3Int>();
        Vector3[] newVertices = new Vector3[8 * (nrOfSections * (nrOfSections - 1)) / 2 + 2 + 4 * nrOfSections];

        int offset = newVertices.Length - 1;
        newVertices[0] = vertices[0];
        newVertices[offset] = vertices[vertices.Length - 1];

        for (int ch = 0; ch < chunks.Length; ch++)
        {
            chunks[ch] = new Chunk(chunkSize);
        }

        chunks[0].AddVertex(newVertices[0]);
        chunks[1].AddVertex(newVertices[0]);
        chunks[2].AddVertex(newVertices[0]);
        chunks[3].AddVertex(newVertices[0]);
        chunks[chunks.Length - 1].AddVertex(newVertices[offset]);
        chunks[chunks.Length - 2].AddVertex(newVertices[offset]);
        chunks[chunks.Length - 3].AddVertex(newVertices[offset]);
        chunks[chunks.Length - 4].AddVertex(newVertices[offset]);

        int c = 1;
        int startingIndex = 1;
        for (int s = 1; s <= nrOfSections; s++)
        {
            for (int v = 1; v < 5; v++)
            {
                for (int vs = 0; vs < s; vs++)
                {
                    //Determine ID of the correct chunk
                    int chunkLayer = (s - 1) / chunkSize;
                    int parallelogramInLayer = vs / chunkSize;
                    //v is the chunk's vertical slice
                    int mainChunkID = (chunkLayer) * (chunkLayer) * 4                       //Chunks in higher layer
                    + (2 * chunkLayer + 1) * ((v + ((vs == 0) ? 2 : 3)) % 4)                //Chunks in previous slices
                    + chunkSize * parallelogramInLayer                                      //Chunks in this slice
                    + ((vs % chunkSize > s % chunkSize) ? 1 : 0);                           //Decides, which half of the parallelogram to choose

                    if (v == 1 && vs == 0) mainChunkID += 2 * chunkSize;


                    //Section on upper half
                    newVertices[c] = vertices[0]
                        + s * (vertices[v] - vertices[0]) / nrOfSections
                        + vs * (vertices[v % 4 + 1] - vertices[v]) / nrOfSections;

                    chunks[mainChunkID].AddVertex(newVertices[c]);
                    //Add vertex to all adjacent chunks
                    if (vs % chunkSize == 0)
                    {
                        chunks[(mainChunkID - 4 * chunkLayer * chunkLayer + 1) % (4 * (2 * chunkLayer + 1)) + 4 * chunkLayer * chunkLayer].AddVertex(newVertices[c]);
                    }
                    if (s % chunkSize == vs % chunkSize)
                    {
                        chunks[mainChunkID + 1].AddVertex(newVertices[c]);
                    }
                    if (s % chunkSize == 0)
                    {
                        if (s < nrOfSections) chunks[(chunkLayer + 1) * (chunkLayer + 1) + 2 * (parallelogramInLayer + 1) + (v - 1)].AddVertex(newVertices[c]);
                        else
                        {
                            //Find inverse chunk
                        }
                    }

                    //Section on lower half
                    if (startingIndex + 4 * s < newVertices.Length / 2)
                    {
                        newVertices[offset - c] = vertices[5]
                            + s * (vertices[5 - v] - vertices[5]) / nrOfSections
                            + (s - vs - 1) * (vertices[(5 - v) % 4 + 1] - vertices[5 - v]) / nrOfSections;
                        chunks[chunks.Length - 1 - mainChunkID].AddVertexToBack(newVertices[offset - c]);

                    }

                    if (c > startingIndex)
                    {
                        //Make triangles above vertex
                        int rightParent = c - (4 * (s - 1)) - (v - 1) - 1;                                                                  //Upper hemisphere
                        int leftParent = (rightParent == startingIndex - 1) ? startingIndex - 4 * (s - 1) : rightParent + 1;
                        Vector3Int tri = new Vector3Int(((vs != 0 || s == 1) ? rightParent : leftParent), c - 1, c);
                        newTriangles.Add(tri);
                        newTriangles.Add(new Vector3Int(offset - tri.x + ((vs == 0 && s > 1) ? 1 : 0), offset - tri.y, offset - tri.z));    //Lower hemisphere

                        //Make triangles below vertex
                        if (vs != 0)
                        {
                            Vector3Int downTri = new Vector3Int(leftParent, rightParent, c);                                                //Upper hemisphere
                            newTriangles.Add(downTri);
                            newTriangles.Add(new Vector3Int(offset - downTri.x, offset - downTri.y,                                         //Lower hemisphere
                                ((v == 4 && vs == s - 1 && s > 1) ? offset - downTri.y - 1 : offset - downTri.z - ((vs == s - 1 && s > 1) ? 1 : 0))));
                        }
                    }
                    c++;
                }
            }
            //Finish layer
            Vector3Int finishingTri = new Vector3Int((s != 1) ? startingIndex - 4 * (s - 1) : startingIndex - 1, --c, startingIndex);
            newTriangles.Add(finishingTri);
            newTriangles.Add(new Vector3Int(offset - finishingTri.z + 1, offset - finishingTri.y, offset - finishingTri.z));
            startingIndex = ++c;
        }

        vertices = newVertices;
        triangles = new int[newTriangles.Count * 3];
        int i = 0;
        foreach (Vector3Int t in newTriangles)
        {
            triangles[i++] = t.x;
            triangles[i++] = t.y;
            triangles[i++] = t.z;
        }
    }

    public override void UpdateMesh(Vector3 direction, float radius)
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


    public override int NrOfVerteces()
    {
        return vertices.Length;
    }

    public override Vector3 GetVertex(int index)
    {
        return new Vector3(vertices[index].x, vertices[index].y, vertices[index].z);
    }

    public override void SetVertex(int index, Vector3 position)
    {
        vertices[index] = position;
    }
}