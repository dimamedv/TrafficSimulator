using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoad : AbstractRoad
{
    public List<float> prefixSumSegments = new List<float>(); // ������ ���������� ����. ��������� ������� - ����� ���� ������
    public bool createCrossRoadEntrance;
    public GameObject crossRoadEntrancePrefab;
    public GameObject crossRoadEntrance;
    public bool isReadyMadePoints;

    private void Update()
    {
        BuildRoad();
    }

    public override void BuildRoad(bool endIteration = true, bool isReadyMadePoints = false)
    {
        if (isReadyMadePoints == false) {
            ClearLists();
            if (isStraight)
            {
                formingPoint.transform.position =
                    MyMath.GetMidPoint(startPost.transform.position, endPost.transform.position);
                CalculateQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
                    endPost.transform.position, 1);
            }
            else
            {
                CalculateQuadraticBezierCurve(startPost.transform.position, formingPoint.transform.position,
                endPost.transform.position, details);
            }
        }
        CalculateLengthOfSimpleRoad();

        if (createCrossRoadEntrance && !transform.Find("CrossRoadEntrance"))
        {
            crossRoadEntrance = Instantiate(crossRoadEntrancePrefab, endPost.transform.position,
                endPost.transform.rotation);
            crossRoadEntrance.transform.SetParent(gameObject.transform);
            crossRoadEntrance.transform.name = "CrossRoadEntrance";
        }
        else if (!createCrossRoadEntrance && transform.Find("CrossRoadEntrance"))
        {
            Destroy(crossRoadEntrance);
            CrossRoadEntrance.EntrancesList.Remove(crossRoadEntrance);
            crossRoadEntrance = null;
        }

        if (gameObject.GetComponent<MeshVisualization>())
            gameObject.GetComponent<MeshVisualization>().RenderingRoad(points);
    }


    // ������������ ����� ������, �������� ������ ���������� ����
    private void CalculateLengthOfSimpleRoad()
    {
        prefixSumSegments.Add(0.0f);
        for (int i = 0; i < points.Count - 1; i++)
            prefixSumSegments.Add(prefixSumSegments[i] + MyMath.getDistance(points[i], points[i + 1]));
    }
}
