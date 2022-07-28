using UnityEngine;

namespace Skripts
{
    public class MyMath
    {
        public Vector3 CalculateMidPoint(Vector3 v1, Vector3 v2)
        {
            return v1 + (v2 - v1) / 2;
        }
    }
}