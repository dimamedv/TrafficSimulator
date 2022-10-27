using System.Collections.Generic;
using System;
using UnityEngine;
using System.Security.Cryptography;

public class Box
{
    private float minX, maxX, minZ, maxZ;

    public void createByCurve(List<Vector3> points)
    {
        minX = points[0].x;
        maxX = points[0].x;
        minZ = points[0].z;
        maxZ = points[0].z;
        
        foreach (var point in points)
        {
            if (point.x > maxX)
                maxX = point.x;
            if (point.x < minX)
                minX = point.x;
            if (point.z > maxZ)
                maxZ = point.z;
            if (point.z < minZ)
                minZ = point.z;
        }
    }

    public bool hasIntersection(Box box)
    {
        return pointIsInside(box.minX, box.minZ) || pointIsInside(box.minX, box.maxZ) ||
               pointIsInside(box.maxX, box.minZ) || pointIsInside(box.maxX, box.maxZ);
    }

    public bool pointIsInside(float x, float z)
    {
        return (x > minX && x < maxX && z > minZ && z < maxZ);
    }
}

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


    public static int binarySearch(ref List<float> a, int size, float x)
    {
        int left = -1;
        int right = size - 1;
        while (right - left > 1)
        {
            int mid = left + (right - left) / 2;

            if (a[mid] < x)
                left = mid;
            else
                right = mid;
        }

        return right;
    }

    public Vector3 getBezieCurveIntersection(List<Vector3> points1, List<Vector3> points2)
    {
        Box box1 = new Box();
        Box box2 = new Box();
        
        box1.createByCurve(points1);
        box2.createByCurve(points2);

        return Vector3.zero;
    }

}