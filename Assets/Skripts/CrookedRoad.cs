 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrookedRoad : MonoBehaviour
{
    // Образующие точки кривой Безье
    public Transform p0;
    public Transform p1;
    public Transform p2;
    // Количество фрагментов дороги (Детализация)
    public int details;
    // Ширина дороги
    public float roadWidth;
    // Координаты точек Безье
    private List<Vector3> bezierPoints;
    // Вершины дороги
    private List<Vector3> vertexRoad;

    // Start is called before the first frame update
    void Start()
    {
        bezierPoints = new List<Vector3>();
        vertexRoad = new List<Vector3>();
        
        // Создаем массив из формирующих точек кривой безье
        DrawQuadraticBezierCurve(p0.position, p1.position, p2.position);

        // По ним получаем координаты точек, которые являются изломами дороги
        getVertexPoints();

        // По этим координатам создаем меш дороги
        CreateMesh();
    }

    // Составляет кривую Безье по трем координатам
    private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        float t = 0f;
        Vector3 B;
        for (int i = 0; i < details; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            bezierPoints.Add(B);
            t += (1 / (float)details);
        }
        bezierPoints.Add(point2);
    }

    private void getVertexPoints()
    {
        GetEndpoints(bezierPoints[0], bezierPoints[1]);

        for (int i = 1; i < details; i++)
            GetBendOfRoad(bezierPoints[i - 1], bezierPoints[i], bezierPoints[i + 1]);

        GetEndpoints(bezierPoints[bezierPoints.Count - 1], bezierPoints[bezierPoints.Count - 2]);
    }

    private float getDistance(Vector3 v1, Vector3 v2) { 
        double x = v1.x - v2.x;
        double z = v1.z - v2.z;
        return (float)Math.Sqrt(x * x + z * z);
    }

    /* Добавляет сначала внешнюю точку в List vertexRoad, потом внутренюю
    ** a - Первая точка принадлежащая кривой Безье
    ** b - Вторая (следующая) точка принадлежащая кривой Безье
    ** offset - Смещение вершины дороги, относительно a
    */
    private void addOuterVertexFirst(Vector3 a, Vector3 b, Vector3 offset)
    {
        Vector3 v1 = a + offset;
        Vector3 v2 = a - offset;

        if (getDistance(v1, b) > getDistance(v2, b))
        {
            vertexRoad.Add(v1);
            vertexRoad.Add(v2);
        }
        else
        {
            vertexRoad.Add(v2);
            vertexRoad.Add(v1);
        }
    }

    // Записывает координаты вершин для конца дороги в лист vertexRoad
    private void GetEndpoints(Vector3 a, Vector3 b)
    {
        Vector3 delta = b - a;
        double arctgA = Math.Atan(delta.x / delta.z);
        // Скорее всего можно упростить
        Vector3 offset = new Vector3((float)Math.Cos(-arctgA) * roadWidth, 0f, (float)Math.Sin(-arctgA) * roadWidth);
        addOuterVertexFirst(a, b, offset);
    }

    // Записывает координаты вершин на месте изгиба дороги в лист vertexRoad
    private void GetBendOfRoad(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 delta = b - a;
        double arctgA = Math.Atan(delta.x / delta.z);
        Vector3 AB = b - a;
        Vector3 BC = b - c;
        double lenAB = Math.Sqrt(AB.x * AB.x + AB.z * AB.z);
        double lenBC = Math.Sqrt(BC.x * BC.x + BC.z * BC.z);
        // Арккосинус угла p1p2p3 деленный на два
        double arccos = Math.Acos((AB.x * BC.x + AB.z * BC.z) / (lenAB * lenBC)) / 2;
        // Скорее всего можно упростить
        Vector3 offset = new Vector3((float)Math.Cos(arccos - arctgA + Math.PI / 2) * roadWidth, 0f, (float)Math.Sin(arccos - arctgA + Math.PI/2) * roadWidth);

        addOuterVertexFirst(b, c, offset);
    }

    private void CreateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        // Скорее всего можно упростить
        Vector3[] V = new Vector3[vertexRoad.Count];
        for (int i = 0; i < vertexRoad.Count; i++) V[i] = vertexRoad[i];
        mesh.vertices = V;

        // Количество адресов будет равно количеству вершин в треугольнике на количество треугольников
        // в квадрате. А так как полигоны нужно сделать с двух сторон, то домножаем еще на 2
        int[] triangles = new int[bezierPoints.Count * 3 * 2 * 2];

        // Сначала адреса вершин треугольника должны возрастать, а потом убывать
        for (int i = 0; i < (bezierPoints.Count - 1) * 2 * 2; i++)
        {
            // Номер треугольника
            int j = i / 2;
            // Отоброжение одной стороны треугольника
            for (int k = 0; k < 3; k++) triangles[i * 3 + k] = j++;
            i++;
            // Отображение другой стороны треугольника
            for (int k = 0; k < 3; k++) triangles[i * 3 + k] = --j;
        }

        mesh.triangles = triangles;
    }
}
