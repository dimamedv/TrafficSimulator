using System.Collections.Generic;
using System;
using UnityEngine;

namespace Skripts
{
    public class MyMath 
    {
        public static float eps = 0.001f;
        public Vector3 CalculateMidPoint(Vector3 v1, Vector3 v2)
        {
            return v1 + (v2 - v1) / 2;
        }

        public static int binarySearch(ref List<float> a, float x)
        {
            int left = 0;
            int right = a.Count - 1;
            while (Math.Abs(left - right) <= eps)
            {
                int mid = (left + right) / 2;
                if (Math.Abs(a[mid] - x) < eps)  left = mid + 1;
                else if (Math.Abs(a[mid] - x) > eps)  right = mid - 1;
                else return mid;
            }
            return left;
        }
    }
}