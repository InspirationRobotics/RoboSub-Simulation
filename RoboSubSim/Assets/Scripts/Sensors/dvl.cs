using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dvl : MonoBehaviour

{
    public Rigidbody DVL;
    public Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody DVL = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        vel = DVL.velocity;
        print(vel.magnitude);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), vel.magnitude.ToString());
    }

}