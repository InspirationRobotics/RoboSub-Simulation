using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imu : MonoBehaviour, ISensor
{
    public GameObject device;
    public float Roll, Pitch, Yaw;
    public JSONObject pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void update ()
    {
        Roll = device.transform.localRotation.eulerAngles.z;
        Pitch = device.transform.localRotation.eulerAngles.x;
        Yaw = device.transform.localRotation.eulerAngles.y;
        pos = new JSONObject();
        pos.AddField("Roll", Roll);
        pos.AddField("Pitch", Pitch);
        pos.AddField("Yaw", Yaw);


        
    }
    public JSONObject RequestObs(JSONObject json)
    {
        json.AddField(transform.name, "imuuu");
        return json;
    }
}
