using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeometricUtil
{
    public static Vector3 Rotate(Vector3 original, Vector3 axis, float angle)
    {
        float l = original.magnitude;
        Vector3 b = Vector3.Cross(original.normalized, axis.normalized).normalized * l * Mathf.Tan(Mathf.PI * (angle / 180f));
        return (original + b).normalized * l;
    }
}
