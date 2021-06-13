using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Submarine : MonoBehaviour
{
    Rigidbody rb;
    public float waterLevel = 0f;
    public float waterDrag = 5f;

    public float upForce = 0f;
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
        if (upForce != 0f) { rb.AddRelativeForce(Vector3.up * upForce, ForceMode.Acceleration); }
        if (forwardForce != 0f) { rb.AddRelativeForce(Vector3.right * forwardForce, ForceMode.Acceleration); }
        if (rollForce != 0f) { rb.AddRelativeTorque(Vector3.right * rollForce, ForceMode.Acceleration); }
        if (pitchForce != 0f) { rb.AddRelativeTorque(Vector3.forward * pitchForce, ForceMode.Acceleration); }
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

    public void RestorePosRot()
    {
        rb.position = startPos;
        rb.rotation = startRot;
    }

}
