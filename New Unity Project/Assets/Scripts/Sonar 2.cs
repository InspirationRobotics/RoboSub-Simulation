// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public class Sonar : MonoBehaviour
// {
//     public Transform sonar;

//     public float dist;
//     public GUIStyle style;
//     // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
//     void Start(){
//         style.fontSize = 50;
//     }
//     void Update()
//     {
//         // Bit shift the index of the layer (8) to get a bit mask
//         int layerMask = 0;

//         // This would cast rays only against colliders in layer 8.
//         // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
//         // layerMask = ~layerMask;

//         RaycastHit hit;
//         // Does the ray intersect any objects excluding the player layer
//         if (Physics.Raycast(sonar.position, sonar.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
//         {
//             dist = hit.distance;
//         }
//         else
//         {
//             dist = 2;
//         }
//     }
//     private void OnGUI(){
//         GUI.Label(new Rect(50, 150, 200, 200), "Dist = " + dist, style);
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Sonar : MonoBehaviour
// {
//     public Transform sonar;
//     public bool measuredDistance;
//     public GUIStyle style;
//     // public string Tag;
//     void Start()
//     {
//         // StartCoroutine(UpdateSensor());
//         style.fontSize = 50;
//     }

//     public void UpdateSensor()
//     {
//         Ray ray = new Ray(sonar.position, Vector3.forward);
//         Debug.DrawRay(sonar.position, Vector3.forward*10, Color.green, 10, false);               
//         if(Physics.Raycast(ray, out RaycastHit hit)){
//             measuredDistance = Physics.Raycast(ray, out RaycastHit hit);
//         }
//         else{
//             measuredDistance = 300;
//         }
             
//     } 

//     public void OnGUI ()
//     {
//         GUI.Label(new Rect(50, 150, 200, 200), "Dist = " + measuredDistance, style);
//     }
// }