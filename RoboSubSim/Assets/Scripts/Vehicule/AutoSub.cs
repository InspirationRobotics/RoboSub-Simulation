using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSub : MonoBehaviour
{
    public Submarine sub;
    public NavPath path;
    int progressOnPath;
    Vector3[] vertices;


    float t = 0;

    bool rotating = true; // first state
    bool pitching = false; // second state
    bool forwarding = false; // third state
    bool rotReset = false; // last state (before moving to the next point)
    bool rolling = false;

    public void Init()
    {
        vertices = path.GetVertices();
    }

    public void Update()
    {
        // proceed to auto drive
        if (vertices == null || vertices.Length == 0) { return; }

        Vector3 targetVertex = vertices[progressOnPath];
        Vector3 relativePos = sub.transform.InverseTransformVector(targetVertex - sub.transform.position);
        Vector3 relativePosNorm = relativePos.normalized;

        // Debug.Log(relativePos.ToString());

        if (rotReset)
        {
            Quaternion rot = sub.rb.rotation;

            if (pitching)
            {
                if (Mathf.Abs(rot.z) < 0.01 && IsStable(0.01f, 0.01f)) { pitching = false; rolling = true; sub.ResetForces(); }
                else { sub.pitchForce = rot.z * 0.1f; }
            }
            else if (rolling)
            {

                if (Mathf.Abs(rot.x) < 0.01 && IsStable(0.01f, 0.01f)) { rolling = false; pitching = true; sub.ResetForces(); }
                else { sub.rollForce = rot.x * 0.05f; }

                if (IsRotReset())
                {
                    progressOnPath = (progressOnPath + 1) % vertices.Length;
                    rotReset = false;
                    rolling = false;
                    rotating = true;
                }

            }
        }

        else if (rotating)
        {
            if (Mathf.Abs(relativePosNorm.x) < 0.05 && IsStable()) { rotating = false; pitching = true; sub.ResetForces(); }
            else { sub.yawForce = relativePosNorm.x * 0.1f; }
        }
        else if (pitching)
        {
            if (Mathf.Abs(relativePosNorm.y) < 0.05 && IsStable()) { pitching = false; forwarding = true; sub.ResetForces(); }
            else { sub.pitchForce = relativePosNorm.y * -0.1f; }
        }
        else if (forwarding)
        {
            if (Mathf.Abs(relativePosNorm.z) < 0.05 && IsStable()) { forwarding = false; rotating = true; sub.ResetForces(); }
            else { sub.forwardForce = relativePosNorm.z * 0.1f; }

            if (isNearbyPoint(targetVertex, 0.5f) && IsStable())
            {
                sub.ResetForces();
                forwarding = false;
                rotReset = true;
                pitching = true;
            }
        }

    }

    public bool IsStable(float velTolerance = 0.05f, float angVelTolerance = 0.05f)
    {
        if (sub.rb.velocity.magnitude < velTolerance && sub.rb.angularVelocity.magnitude < angVelTolerance)
            return true;
        else
            return false;
    }

    public bool IsRotReset(float tolerance = 0.05f)
    {
        Quaternion rot = sub.rb.rotation;

        if (rot.x < tolerance && rot.z < tolerance)
            return true;
        else
            return false;
    }


    public bool isNearbyPoint(Vector3 point, float tolerance = 1)
    {
        if (Vector3.Distance(point, sub.transform.position) < tolerance)
            return true;
        else
            return false;
    }


    public int GetProgressOnPath() { return progressOnPath; }

}