using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtilities
{

    public static float GetDistanceIgnoringY(Vector3 a, Vector3 b)
    {
        Vector2 a2 = new Vector2() { x = a.x, y = a.z };
        Vector2 b2 = new Vector2() { x = b.x, y = b.z };
        return Vector2.Distance(a2, b2);
    }

}