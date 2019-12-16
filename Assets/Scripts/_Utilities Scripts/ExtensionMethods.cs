using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 RandomVector(this ref Vector3 v)
    {
        v.x = Random.Range(0, 360);
        v.y = Random.Range(0, 360);
        v.z = Random.Range(0, 360);
        return v;
    }
}
