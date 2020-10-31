using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeshGenerator : MonoBehaviour
{
    public abstract void UpdateMesh(Vector3 direction, float radius);

    public abstract int NrOfVerteces();

    public abstract Vector3 GetVertex(int index);

    public abstract void SetVertex(int index, Vector3 position);
}
