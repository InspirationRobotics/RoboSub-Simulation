using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaded : MonoBehaviour
{
    void Start()
    {
        SandboxServer server = GameObject.FindObjectOfType<SandboxServer>();

        if (server)
        {
            // server.OnSceneLoaded();
        }
    }
}
