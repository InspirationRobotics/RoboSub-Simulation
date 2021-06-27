using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSub : MonoBehaviour
{
    public Submarine sub;
    public NavPath path;
    int progressOnPath;
    Vector3[] vertices;

    public void Init()
    {
        vertices = path.GetVertices();
    }

    public void Update()
    {
        // proceed to auto drive
        if (vertices == null || vertices.Length == 0) { return; }

        Vector3 targetVertex = vertices[progressOnPath];
        Quaternion rotation = Quaternion.LookRotation(targetVertex - sub.transform.position, Vector3.up);

        // Quaternion.

        sub.transform.rotation = rotation;


        // Debug.Log((sub.transform.rotation - rotation).ToString());



    }


    public bool isNearbyPoint(Vector3 point, float tolerance = 1)
    {
        if (Vector3.Distance(point, sub.transform.position) < tolerance) { return true; }
        else { return false; }
    }


    public int GetProgressOnPath() { return progressOnPath; }

}