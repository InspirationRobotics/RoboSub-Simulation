using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSub : MonoBehaviour
{
    public Submarine sub;
    public NavPath path;
    int progressOnPath;
    Vector3[] vertices;


    bool rotating = true; // first state
    bool leveling = false; // second state
    bool forwarding = false; // third state
    bool rotReset = false; // last state (before moving to the next point)
    bool pitching = false;
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

        if (rotReset) // get the sub in a stable orientation
        {
            Quaternion rot = sub.rb.rotation;

            if (rolling)
            {

                if (Mathf.Abs(rot.x) < 0.01 && IsStable(0.01f, 0.01f)) { rolling = false; sub.ResetForces(); }
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
            if (Mathf.Abs(relativePos.x) < 0.05 && IsStable()) { rotating = false; leveling = true; sub.ResetForces(); }
            else { sub.yawForce = relativePos.x * 0.05f; }
        }
        else if (leveling)
        {
            if (Mathf.Abs(relativePos.y) < 0.05 && IsStable()) { leveling = false; forwarding = true; sub.ResetForces(); }
            else { sub.upForce = relativePos.y * 0.1f; }
        }
        else if (forwarding)
        {
            if (Mathf.Abs(relativePos.z) < 0.05 && IsStable()) { forwarding = false; rotating = true; sub.ResetForces(); }
            else { sub.forwardForce = relativePosNorm.z * 0.3f; }

            if (isNearbyPoint(targetVertex, 1f) && IsStable())
            {
                sub.ResetForces();
                forwarding = false;
                rotReset = true;
                rolling = true;
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