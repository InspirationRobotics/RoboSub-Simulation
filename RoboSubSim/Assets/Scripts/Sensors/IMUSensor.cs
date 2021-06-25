using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMUSensor : MonoBehaviour, ISensor
{
    public float Roll, Pitch, Yaw;
    public Vector3 position = Vector3.zero;
    public Vector3 prevPosition = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public Vector3 prevVelocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public float prevTime = 0f;

    public JSONObject RequestObs(JSONObject json)
    {
        float now = Time.time;

        Roll = transform.rotation.eulerAngles.z;
        Pitch = transform.rotation.eulerAngles.x;
        Yaw = transform.rotation.eulerAngles.y;

        position = transform.position;
        velocity = (prevPosition - position) / (now - prevTime);
        acceleration = (prevVelocity - velocity) / (now - prevTime);
        prevPosition = position;
        prevVelocity = velocity;
        prevTime = now;

        JSONObject info = new JSONObject();
        info.AddField("roll", Roll);
        info.AddField("pitch", Pitch);
        info.AddField("yaw", Yaw);
        info.AddField("acc_x", acceleration.x);
        info.AddField("acc_y", acceleration.y);
        info.AddField("acc_z", acceleration.z);


        json.AddField(transform.name, info);
        return json;
    }
}
