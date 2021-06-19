using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCam : MonoBehaviour
{
    public Transform SubTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - SubTransform.position;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = SubTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }
}
