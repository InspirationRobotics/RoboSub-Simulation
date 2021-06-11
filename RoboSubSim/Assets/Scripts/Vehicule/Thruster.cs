using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Vector3 thrustVector;

    void Awake()
    {
        thrustVector = thrustVector.normalized;
    }
}
