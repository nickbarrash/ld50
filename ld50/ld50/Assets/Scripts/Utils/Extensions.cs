using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector3 Rotate(this Vector3 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static T Last<T>(this List<T> list) {
        if (list == null || list.Count == 0)
            return default(T);

        return list[list.Count - 1];
    }

    public static long EpochSeconds() {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
