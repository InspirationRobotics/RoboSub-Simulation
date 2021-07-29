using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSpawner : MonoBehaviour
{
    public GameObject subPrefab;
    public GameObject demoSubPrefab;
    public GameObject mainCameraGO;
    public Transform spawnPoint;
    public NavPath demoPath;

    List<GameObject> subs = new List<GameObject>();

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;

        Debug.LogError("couldn't find: " + withName);
        return null;
    }

    public GameObject SpawnNewSub(tk.JsonTcpClient _TcpClient, tk.JsonUdpClient _UdpClient)
    {
        if (subPrefab == null)
        {
            Debug.LogError("No subPrefab set in SubmarineSpawner");
            return null;
        }

        bool isDemoSub = _TcpClient == null;

        GameObject go;
        if (isDemoSub)
            go = GameObject.Instantiate(demoSubPrefab) as GameObject;
        else
            go = GameObject.Instantiate(subPrefab) as GameObject;

        go.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        subs.Add(go);

        GameObject TcpUdpClientObj = getChildGameObject(go, "TcpUdpClient");
        tk.TcpUdpSubHandler subHandler = TcpUdpClientObj.GetComponent<tk.TcpUdpSubHandler>();

        // without this it will not connect.
        TcpUdpClientObj.SetActive(true);

        // now set the connection settings.
        subHandler.Init(_TcpClient, _UdpClient);

        if (_TcpClient == null)
        {
            AutoSub autoSub = go.GetComponentInChildren<AutoSub>();
            if (autoSub != null) // an AutoSub class should exist in demoSub
            {
                autoSub.path = demoPath;
                autoSub.Init();
            }
            else
                Debug.LogError("Couldn't find AutoSub class in demo sub");
        }

        manageCamera(isDemoSub);
        return go;
    }

    public void RemoveSub(tk.JsonTcpClient client)
    {

        GameObject toRemove = null;

        foreach (GameObject go in subs)
        {
            GameObject TcpUdpClientObj = getChildGameObject(go, "TcpUdpClient");

            if (TcpUdpClientObj != null)
            {
                tk.TcpUdpSubHandler handler = TcpUdpClientObj.GetComponent<tk.TcpUdpSubHandler>();

                if (handler != null && handler.GetClient() == client)
                {
                    toRemove = go;
                }
            }
        }

        if (toRemove != null)
        {
            subs.Remove(toRemove);
            GameObject.Destroy(toRemove);
        }
    }

    public void manageCamera(bool isDemoSub)
    {
        if (subs.Count == 0) { return; }

        GameObject sub = subs[subs.Count - 1];

        if (isDemoSub)
        {
            mainCameraGO.SetActive(false);
        }
        else
        {
            CameraFollow cm = mainCameraGO.GetComponent<CameraFollow>();
            GameObject camTm = getChildGameObject(sub, "CameraTm");
            cm.target = camTm.transform;
        }


        // DrawSonar ds = mainCameraGO.GetComponent<DrawSonar>();
        // ds.subObj = sub;
        // ds.sonarSensors = sub.GetComponentsInChildren<SonarSensor>();

    }

}
