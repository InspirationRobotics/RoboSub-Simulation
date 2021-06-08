using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    Rigidbody rb;
    float waterLevel = 0f; // not needed for the moment
    GameObject[] thrustersGO;
    Thruster[] thrusters;
    float[] thrustForces;

    void awake()
    {
        // get the thrusters objects
        thrusters = new Thruster[thrustersGO.Length];
        thrustForces = new float[thrustersGO.Length];

        for (int i = 0; i < thrustersGO.Length; i++)
        {
            thrusters[i] = thrustersGO[i].GetComponent<Thruster>();
        }
    }

    void FixedUpdate()
    {
        for(int i = 0; i < thrustForces.Length; i++)
        {
            float thForce = thrustForces[i];
            if (thForce != 0)
            {
                ApplyForceThruster(i, thForce);
            }
        }
    }

    void ApplyForceThruster(int thIndex, float force)
    {
        if (thIndex >= thrusters.Length)
        {
            Debug.LogError("Thruster index out of range");
            return;
        }

        Thruster th = thrusters[thIndex];
        rb.AddForceAtPosition(th.thrustVector * force, th.transform.position, ForceMode.Acceleration);

    }

}