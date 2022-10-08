using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoad : AbstractRoad
{
    public List<float> prefixSumSegments = new List<float>(); // ������ ���������� ����. ��������� ������� - ����� ���� ������

    public override void BuildRoad(bool endIteration = true)
    {
        CalculateLengthOfSimpleRoad();
    }


    // ������������ ����� ������, �������� ������ ���������� ����
    private void CalculateLengthOfSimpleRoad()
    {
        prefixSumSegments.Add(0.0f);
        for (int i = 0; i < points.Count - 1; i++)
            prefixSumSegments.Add(prefixSumSegments[i] + MyMath.getDistance(points[i], points[i + 1]));
    }
}
