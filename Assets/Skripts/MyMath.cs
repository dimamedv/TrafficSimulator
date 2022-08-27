using System.Collections.Generic;
using System;
using UnityEngine;
using System.Security.Cryptography;


public class MyMath
{
    public static Vector3 GetMidPoint(Vector3 v1, Vector3 v2)
    {
        return v1 + (v2 - v1) / 2;
    }

    public static float eps = 0.001f;
    
    
    public static float getDistance(Vector3 v1, Vector3 v2)
    {
        double x = v1.x - v2.x;
        double z = v1.z - v2.z;
        return (float)Math.Sqrt(x * x + z * z);
    }
    

    // Это не универсальное решение
    public static int binarySearch(ref List<float> a, int size, float x)
    {
        // Из-за этой строчки
        int left = 1;
        int right = size - 1;
        while (left <= right)
        {
            int mid = (right + left) / 2;
            if (a[mid] - x < eps) left = mid + 1;
            else right = mid - 1;
        }

        return left - 1;
    }
}