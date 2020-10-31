using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinPlanet : MeshGenerator
{
    [SerializeField]
    [Range(0, 1)]
    private float intensity;

    [SerializeField]
    private float baseFrequency;
    [SerializeField]
    private int seed;
    private Vector3 offset;
    [SerializeField]
    private int iterations;


    [SerializeField]
    private MeshGenerator baseShape;

    private MeshGenerator planetMesh;
    void Start()
    {
        planetMesh = Instantiate(baseShape, Vector3.zero, Quaternion.identity);

        System.Random prng = new System.Random(seed);
        offset = new Vector3((float)(200 * (prng.NextDouble() - 0.5)), (float)(200 * (prng.NextDouble() - 0.5)), (float)(200 * (prng.NextDouble() - 0.5)));
        offset = Vector3.zero;

        AddPerlinNoise();
    }

    void Update()
    {

    }

    private void AddPerlinNoise()
    {
        int nrOfVerts = planetMesh.NrOfVerteces();
        for (int i = 0; i < nrOfVerts; i++)
        {
            Vector3 v = planetMesh.GetVertex(i);
            planetMesh.SetVertex(i, SingleVertexPerlin(v));
        }
    }

    private Vector3 SingleVertexPerlin(Vector3 v)
    {
        float perlinOffset = 1;
        float factor = 1;
        for (int i = 0; i < iterations; i++)
        {
            perlinOffset += (intensity / factor) * Noise.PerlinNoise3D((v/* + factor * offset*/) / baseFrequency * factor);
            factor *= 2;
        }

        return v * perlinOffset;
    }

    public override void UpdateMesh(Vector3 direction, float radius)
    {
        planetMesh.UpdateMesh(direction, radius);
    }

    public override void SetVertex(int index, Vector3 position)
    {
        planetMesh.SetVertex(index, position);
    }

    public override int NrOfVerteces()
    {
        return planetMesh.NrOfVerteces();
    }

    public override Vector3 GetVertex(int index)
    {
        return planetMesh.GetVertex(index);
    }
}
