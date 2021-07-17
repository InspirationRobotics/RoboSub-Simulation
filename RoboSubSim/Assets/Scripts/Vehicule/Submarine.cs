using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Submarine : MonoBehaviour
{
    public Rigidbody rb;
    public float waterLevel = 0f;
    public float waterDrag = 1f;

    public float upForce = 0f;
    public float lateralForce = 0f;
    public float forwardForce = 0f;
    public float rollForce = 0f;
    public float pitchForce = 0f;
    public float yawForce = 0f;


    public bool isSubmerged = true;

    // start position and rot
    public Vector3 startPos;
    public Quaternion startRot;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // TODO simulate random waves/currents etc
        float rdmUp = (Random.value - 0.5f) * 0.2f;
        float rdmForward = (Random.value - 0.5f) * 0.2f;
        float rdmLateral = (Random.value - 0.5f) * 0.2f;

        if (upForce != 0f || rdmUp != 0f) { rb.AddRelativeForce(Vector3.up * (upForce + rdmUp), ForceMode.Acceleration); }
        if (lateralForce != 0f) { rb.AddRelativeForce(Vector3.right * (lateralForce + rdmLateral), ForceMode.Acceleration); }
        if (forwardForce != 0f || rdmForward != 0f) { rb.AddRelativeForce(Vector3.forward * (forwardForce + rdmForward), ForceMode.Acceleration); }
        if (rollForce != 0f) { rb.AddRelativeTorque(Vector3.forward * rollForce, ForceMode.Acceleration); }
        if (pitchForce != 0f) { rb.AddRelativeTorque(Vector3.right * pitchForce, ForceMode.Acceleration); }
        if (yawForce != 0f) { rb.AddRelativeTorque(Vector3.up * yawForce, ForceMode.Acceleration); }

        if (transform.position.y < waterLevel)
        {
            isSubmerged = true;

            // when we are underwater, let's assume there is no gravity but drag increase
            rb.useGravity = false;
            rb.drag = waterDrag;
        }

        else
        {
            isSubmerged = false;

            rb.useGravity = true;
            rb.drag = 0;
        }
    }

    public void ResetForces()
    {
        upForce = 0f;
        forwardForce = 0f;
        rollForce = 0f;
        pitchForce = 0f;
        yawForce = 0f;
    }

    public void RestorePosRot()
    {
        rb.position = startPos;
        rb.rotation = startRot;
    }

}
