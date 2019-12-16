using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingUtilities
{
    const int numViewDirections = 320;
    const int numFrontDirections = 15;
    public static readonly Vector3[] allDirections;
    public static readonly Vector3[] frontDirections;


    static FlockingUtilities()
    {
        allDirections = new Vector3[numViewDirections];
        frontDirections = new Vector3[numFrontDirections];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numViewDirections; i++)
        {
            float t = (float)i / numViewDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            allDirections[i] = new Vector3(x, y, z);

            //populates a much smaller array that is going to be used to perform collision-course checks
            if (i < numFrontDirections)
                frontDirections[i] = new Vector3(x, y, z);
        }
    }
}