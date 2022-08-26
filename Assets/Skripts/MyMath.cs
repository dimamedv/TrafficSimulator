using System.Collections.Generic;
using System;
using UnityEngine;
using System.Security.Cryptography;


public class MyMath
{
    public static Vector3 CalculateMidPoint(Vector3 v1, Vector3 v2)
    {
        return v1 + (v2 - v1) / 2;
    }

    public static float eps = 0.001f;

    // Это не универсальное решение
    public static int binarySearch(ref List<float> a, int size, float x)
    {
        // Из-за этой строчки
        int left = 1;
        int right = size - 1;
        while (left < right)
        {
            int mid = (left + right) / 2;
            if (a[mid] - x <= eps) left = mid + 1;
            else right = mid - 1;
        }

        return left - 1;
    }
}