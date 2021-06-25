using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVLSensor : MonoBehaviour, ISensor
{
    public Vector3 velocity = Vector3.zero;
    public Vector3 position = Vector3.zero;
    public Vector3 prevPosition = Vector3.zero;
    public float prevTime = 0f;

    public JSONObject RequestObs(JSONObject json)
    {
        position = transform.position;
        float now = Time.time;
        velocity = (prevPosition - position) / (prevTime - now);
        prevPosition = position;
        prevTime = now;

        JSONObject velJSON = new JSONObject();
        velJSON.AddField("vel_x", velocity.x);
        velJSON.AddField("vel_y", velocity.y);
        velJSON.AddField("vel_z", velocity.z);

        json.AddField(transform.name, velJSON);
        return json;
    }

}