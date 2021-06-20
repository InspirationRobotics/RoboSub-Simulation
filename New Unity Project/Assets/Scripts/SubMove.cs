using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMove : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Mouse Y");

        rigidbody.AddForce(Vector3.forward * speed * forwardInput);
        
        rigidbody.AddForce(Vector3.right * speed * sideInput);
     
        // rigidbody.AddForce(Vector3.up * speed * verticalInput);
        
        rigidbody.AddForce(Vector3.zero);


    }
}
