using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPath : MonoBehaviour
{
    public PathPoint[] pathPoints;

    public Vector3[] GetVertices()
    {
        Vector3[] vertices = new Vector3[pathPoints.Length];
        for (int i = 0; i < pathPoints.Length; i++)
        {
            vertices[i] = pathPoints[i].GetPosition();
        }
        return vertices;
    }
}
