using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSub : MonoBehaviour
{
    public Submarine sub;
    public NavPath path;
    int progressOnPath;
    Vector3[] vertices;


    public bool rotating = true; // first state
    public bool leveling = false; // second state
    public bool forwarding = false; // third state
    public bool rotReset = false; // last state (before moving to the next point)
    public bool pitching = false;
    public bool rolling = false;

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
            Vector3 rot = sub.rb.rotation.ToEuler();

            if (rolling)
            {

                if (Mathf.Abs(rot.z) < 0.01 && IsStable(0.5f, 0.01f)) //  && IsRotReset()
                {

                    sub.ResetForces();
                    progressOnPath = (progressOnPath + 1) % vertices.Length;
                    rotReset = false;
                    rolling = false;
                    rotating = true;
                }
                else
                {
                    sub.rollForce = rot.z * -0.05f;
                }
            }
        }

        else if (rotating)
        {
            if (Mathf.Abs(relativePos.x) < 0.05 && IsStable()) { rotating = false; leveling = true; sub.ResetForces(); }
            else { sub.yawForce = relativePosNorm.x * 0.05f; }
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

        if (Mathf.Abs(rot.x) < tolerance && Mathf.Abs(rot.z) < tolerance)
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