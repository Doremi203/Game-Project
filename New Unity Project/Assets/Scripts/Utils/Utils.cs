﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomPositionInTorus(float ringRadius, float wallRadius)
    {
        // get a random angle around the ring
        float rndAngle = Random.value * 6.28f; // use radians, saves converting degrees to radians

        // determine position
        float cX = Mathf.Sin(rndAngle);
        float cZ = Mathf.Cos(rndAngle);

        Vector3 ringPos = new Vector3(cX, 0, cZ);
        ringPos *= ringRadius;

        // At any point around the center of the ring
        // a sphere of radius the same as the wallRadius will fit exactly into the torus.
        // Simply get a random point in a sphere of radius wallRadius,
        // then add that to the random center point
        Vector3 sPos = Random.insideUnitSphere * wallRadius;

        return (ringPos + sPos);
    }

    public static float GetDistance2D(Vector3 a, Vector3 b)
    {
        Vector2 a2 = new Vector2() { x = a.x, y = a.z };
        Vector2 b2 = new Vector2() { x = b.x, y = b.z };
        return Vector2.Distance(a2, b2);
    }

}