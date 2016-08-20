using UnityEngine;
using System.Collections;

public static class GP_Math {

    public static float LinearAdjust(float x, float a, float k = 0f)
    {
        return a * x + k;
    }

    public static float QuadraticAdjust(float x, float a, float b, float k = 0f)
    {
        return a * Mathf.Pow(x, 2) + b * x + k;
    }

    public static float CubicAdjust(float x, float a, float b, float c, float k = 0f)
    {
        return a * Mathf.Pow(x, 3) + b * Mathf.Pow(x, 2) + c * x + k;
    }
}
