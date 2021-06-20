using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imu : MonoBehaviour
{
    public GameObject device;
    public GUIStyle style;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnGUI ()
    {
        GUI.Label(new Rect(50, 150, 200, 200), "Rot = " + device.transform.localRotation.eulerAngles.y, style);
    }
}
