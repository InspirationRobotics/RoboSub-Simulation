using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Vector3 relativePos;

    public Vector3 GetThrustVector()
    {
        Debug.Log(transform.rotation * Vector3.up);
        return transform.rotation * Vector3.up;
    }
}
