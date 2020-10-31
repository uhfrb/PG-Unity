using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    private static int nrOfChunks = 0;
    private int size;
    private Vector3[] verteces;
    private int[] tris;

    private int chunkID;

    private Vector3 center;

    private int i, j;               //First and last indices of free section in verteces[]
    private int i_t;                //First index of free section in tris[]

    public Chunk(int size)
    {
        this.size = size + 1;
        verteces = new Vector3[this.size * this.size];
        tris = new int[size * size];
        chunkID = nrOfChunks++;
        center = Vector3.zero;

        i = 0;
        j = verteces.Length - 1;
        i_t = 0;
    }

    public void AddVertex(Vector3 v)
    {
        verteces[i] = v;
        i++;
    }

    public void AddVertexToBack(Vector3 v)
    {
        verteces[j] = v;
        j--;
    }

    public void AddTri(Vector3Int t)
    {
        tris[i_t] = t.x;
        tris[i_t + 1] = t.y;
        tris[i_t + 2] = t.z;
        i_t += 3;
    }
}

