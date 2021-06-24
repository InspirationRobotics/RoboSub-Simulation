using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dvl : MonoBehaviour, ISensor

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
        
    }
    public JSONObject RequestObs(JSONObject json)
    {
        vel = DVL.velocity.magnitude;

        json.AddField(transform.name, vel);
        return json;
    }

}