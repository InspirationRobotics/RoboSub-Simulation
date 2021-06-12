using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tk;
using System.Net;
using System.Net.Sockets;
using System;

[RequireComponent(typeof(tk.TcpServer))]
public class SandboxServer : MonoBehaviour
{
    public string host;
    public int port;

    tk.TcpServer _server = null;

    public GameObject clientTemplateObj = null;

    public void CheckCommandLineConnectArgs()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--host")
            {
                host = args[i + 1];
            }
            else if (args[i] == "--port")
            {
                port = int.Parse(args[i + 1]);
            }
        }
    }

    private void Awake()
    {
        _server = GetComponent<tk.TcpServer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckCommandLineConnectArgs();

        Debug.Log("SDSandbox Server starting.");
        _server.onClientConntedCB += new tk.TcpServer.OnClientConnected(OnClientConnected);
        _server.onClientDisconntedCB += new tk.TcpServer.OnClientDisconnected(OnClientDisconnected);

        _server.Run(host, port);
    }

    // It's our responsibility to create a GameObject with a TcpClient
    // and return it to the server.
    public tk.TcpClient OnClientConnected()
    {
        if (clientTemplateObj == null)
        {
            Debug.LogError("client template object was null.");
            return null;
        }

        if (_server.debug)
            Debug.Log("creating client obj");

        GameObject go = GameObject.Instantiate(clientTemplateObj) as GameObject;
        go.transform.parent = this.transform;

        tk.TcpClient client = go.GetComponent<tk.TcpClient>();
        InitClient(client);

        return client;
    }

    private void InitClient(tk.TcpClient client)
    {
        SubmarineSpawner spawner = GameObject.FindObjectOfType<SubmarineSpawner>();
        if (spawner != null)
        {
            if (_server.debug)
                Debug.Log("spawning car.");

            spawner.SpawnNewSub(client.gameObject.GetComponent<tk.JsonTcpClient>());
        }
    }

    public void OnSceneLoaded()
    {
        List<tk.TcpClient> clients = _server.GetClients();

        foreach (tk.TcpClient client in clients)
        {
            if (_server.debug)
                Debug.Log("init network client.");

            InitClient(client);
        }
    }

    public void OnClientDisconnected(tk.TcpClient client)
    {
        SubmarineSpawner spawner = GameObject.FindObjectOfType<SubmarineSpawner>();
        if (spawner)
        {
            spawner.RemoveSub(client.gameObject.GetComponent<tk.JsonTcpClient>());
        }
        GameObject.Destroy(client.gameObject);
    }
}
