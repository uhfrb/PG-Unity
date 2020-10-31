using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField]
    private MeshGenerator planet;

    [SerializeField]
    private float minDistance;
    void Start()
    {
        transform.LookAt(planet.transform);
        transform.Translate(Vector3.back * minDistance, Space.Self);
        transform.Rotate(10, 0, 0);
    }

    void Update()
    {
        planet.UpdateMesh((planet.gameObject.transform.position - transform.position).normalized, 100f);
    }
}
