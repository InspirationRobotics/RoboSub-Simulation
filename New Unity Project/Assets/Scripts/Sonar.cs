using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Sonardev : MonoBehaviour
{
    public Transform sonar;
    public float measuredDistance;
    public GUIStyle style;
    // public string Tag;
    void Start()
    {
        style.fontSize = 50;
        StartCoroutine(UpdateSensor());
    }

    IEnumerator UpdateSensor()
    {

       while (true){

            Ray ray = new Ray(sonar.position, sonar.rotation * Vector3.right);
            string path = "/Users/Ashi/Desktop/test.txt";

        // DrawRay(sonar.position, Vector3.forward*10, Color.green, 10, false);               
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                measuredDistance = hit.distance;
            } else
            {
                measuredDistance = 1000000;
            }
            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(measuredDistance);
            writer.Close();
            yield return new WaitForSeconds(0.1f);
       } 
    }

    
    // public void OnGUI ()
    // {
    //     GUI.Label(new Rect(50, 150, 200, 200), "Dist = " + measuredDistance, style);
    // }

}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Sonar : MonoBehaviour
// {
//     public Transform sonar;
//     public float measuredDistance;
//     public GUIStyle style;
//     // public string Tag;
//     void Start()
//     {
//         style.fontSize = 50;
//         StartCoroutine(UpdateSensor());
//     }

//     IEnumerator UpdateSensor()
//     {

//        while (true){

//             Ray ray = new Ray(sonar.position, sonar.rotation * Vector3.right);

//         // DrawRay(sonar.position, Vector3.forward*10, Color.green, 10, false);               
//             if(Physics.Raycast(ray, out RaycastHit hit))
//             {
//                 measuredDistance = hit.distance;
//             } else
//             {
//                 measuredDistance = 1000000;
//             }
//             yield return new WaitForSeconds(0.1f);
//        } 
//     }

//     // public void OnGUI ()
//     // {
//     //     GUI.Label(new Rect(50, 150, 200, 200), "Dist = " + measuredDistance, style);
//     // }

// }



