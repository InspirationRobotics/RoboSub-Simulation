using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSpawner : MonoBehaviour
{
    GameObject subPrefab;
    Transform spawnPoint;

    List<GameObject> subs = new List<GameObject>();

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;

        Debug.LogError("couldn't find: " + withName);
        return null;
    }

    public GameObject SpawnNewSub(tk.JsonTcpClient client)
    {
        if (subPrefab == null)
        {
            Debug.LogError("No subPrefab set in SubmarineSpawner");
            return null;
        }

        GameObject go = GameObject.Instantiate(subPrefab) as GameObject;
        go.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        subs.Add(go);

        GameObject TcpClientObj = getChildGameObject(go, "TCPClient");
        if (TcpClientObj != null)
        {
            // without this it will not connect.
            TcpClientObj.SetActive(true);

            // now set the connection settings.
            tk.TcpSubHandler subHandler = TcpClientObj.GetComponent<tk.TcpSubHandler>();

            if (subHandler != null)
                subHandler.Init(client);
        }
        return go;
    }

    public void RemoveSub(tk.JsonTcpClient client)
    {

        GameObject toRemove = null;

        foreach (GameObject go in subs)
        {
            GameObject TcpClientObj = getChildGameObject(go, "TCPClient");

            if (TcpClientObj != null)
            {
                tk.TcpSubHandler handler = TcpClientObj.GetComponent<tk.TcpSubHandler>();

                if (handler != null && handler.GetClient() == client)
                {
                    toRemove = go;
                }
            }
        }

        if (toRemove != null)
        {
            subs.Remove(toRemove);
        }
    }


}
