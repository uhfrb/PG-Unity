using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float PerlinNoise3D(float x, float y, float z)
    {
        int gridX = (int)Mathf.Floor(x);
        int gridY = (int)Mathf.Floor(y);
        int gridZ = (int)Mathf.Floor(z);

        Vector3[] offsets =
        {
            new Vector3(x,y,z) - new Vector3(gridX,gridY,gridZ),
            new Vector3(x,y,z) - new Vector3(gridX,gridY,gridZ + 1),
            new Vector3(x,y,z) - new Vector3(gridX,gridY + 1,gridZ),
            new Vector3(x,y,z) - new Vector3(gridX,gridY + 1,gridZ + 1),
            new Vector3(x,y,z) - new Vector3(gridX + 1,gridY,gridZ),
            new Vector3(x,y,z) - new Vector3(gridX + 1,gridY,gridZ + 1),
            new Vector3(x,y,z) - new Vector3(gridX + 1,gridY + 1,gridZ),
            new Vector3(x,y,z) - new Vector3(gridX + 1,gridY + 1,gridZ + 1)
        };

        float[] intermediate = {
            Vector3.Dot(offsets[0], GetGradientFromGridPoint3D(gridX, gridY, gridZ)),
            Vector3.Dot(offsets[1], GetGradientFromGridPoint3D(gridX, gridY, gridZ + 1)),
            Vector3.Dot(offsets[2], GetGradientFromGridPoint3D(gridX, gridY + 1, gridZ)),
            Vector3.Dot(offsets[3], GetGradientFromGridPoint3D(gridX, gridY + 1, gridZ + 1)),
            Vector3.Dot(offsets[4], GetGradientFromGridPoint3D(gridX + 1, gridY, gridZ)),
            Vector3.Dot(offsets[5], GetGradientFromGridPoint3D(gridX + 1, gridY, gridZ + 1)),
            Vector3.Dot(offsets[6], GetGradientFromGridPoint3D(gridX + 1, gridY + 1, gridZ)),
            Vector3.Dot(offsets[7], GetGradientFromGridPoint3D(gridX + 1, gridY + 1, gridZ + 1)),
        };

        float[] xLerp = {
            PerlinInterpolate(intermediate[0], intermediate[4], x - gridX),
            PerlinInterpolate(intermediate[1], intermediate[5], x - gridX),
            PerlinInterpolate(intermediate[2], intermediate[6], x - gridX),
            PerlinInterpolate(intermediate[3], intermediate[7], x - gridX)
        };

        float[] yLerp = {
            PerlinInterpolate(xLerp[0], xLerp[2], y - gridY),
            PerlinInterpolate(xLerp[1], xLerp[3], y - gridY)
        };

        return PerlinInterpolate(yLerp[0], yLerp[1], z - gridZ);
    }

    private static float PerlinInterpolate(float a, float b, float t)
    {
        return Mathf.Pow(t, 2.0f) * (3.0f - 2.0f * t) * (b - a) + a;
    }

    private static Vector3 GetGradientFromGridPoint3D(int x, int y, int z)
    {
        float random1 = 2920.0f * Mathf.Sin(x * 21942.0f + y * 171324.0f + 74389.0f * z + 8912.0f) * Mathf.Cos(x * 23157.0f + y * 217832.0f + 4362781.0f * z + 9758.0f);
        float random2 = 542.0f * Mathf.Sin(x * 983766.0f + y * 52.0f + 14633.0f * z + 74432.0f) * Mathf.Cos(x * 72245.0f + y * 12343652.0f + 847633.0f * z + 8832.0f); ;
        return new Vector3(Mathf.Sin(random1) * Mathf.Cos(random2), Mathf.Sin(random1) * Mathf.Sin(random2), Mathf.Cos(random2));
    }

    public static float PerlinNoise3D(Vector3 coord)
    {
        return PerlinNoise3D(coord.x, coord.y, coord.z);
    }
}
