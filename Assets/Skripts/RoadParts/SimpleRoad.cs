using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoad : AbstractRoad
{
    public List<float> prefixSumSegments = new List<float>(); // ������ ���������� ����. ��������� ������� - ����� ���� ������

    protected override void BuildRoad(bool endIteration = true)
    {
    }
}
