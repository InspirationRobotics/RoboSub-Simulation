using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace tk
{
    public class TcpUdpSubHandler : MonoBehaviour
    {
        Submarine sub;
        public GameObject subObj;
        public GameObject[] sensorsGO;
        public ISensor[] sensors;
        public bool isDemoSub = false;

        private tk.JsonTcpClient TcpClient;
        private tk.JsonUdpClient UdpClient;

        float timeSinceLastCapture = 0.0f;
        float transferRate = 20.0f;
        bool bResetSub = false;

        void Awake()
        {
            sub = subObj.GetComponent<Submarine>();
            sensors = new ISensor[sensorsGO.Length];

            for (int i = 0; i < sensorsGO.Length; i++)
            {
                sensors[i] = sensorsGO[i].GetComponent<ISensor>();
            }
        }

        public void Start()
        {
            SendSubLoaded();
        }

        public void Init(tk.JsonTcpClient _TcpClient, tk.JsonUdpClient _UdpClient)
        {
            TcpClient = _TcpClient;
            UdpClient = _UdpClient;

            if (TcpClient == null || UdpClient == null)
            {
                isDemoSub = true;
            }

            else
            {
                UdpClient._tcpClient = TcpClient; // needed to send data to the clients

                TcpClient.dispatchInMainThread = false; //too slow to wait.
                TcpClient.dispatcher.Register("get_protocol_version", new tk.Delegates.OnMsgRecv(OnProtocolVersion));
                TcpClient.dispatcher.Register("control", new tk.Delegates.OnMsgRecv(OnControlsRecv));
                TcpClient.dispatcher.Register("reset_sub", new tk.Delegates.OnMsgRecv(OnResetSubRecv));
                // client.dispatcher.Register("set_position", new tk.Delegates.OnMsgRecv(OnSetPosition));
            }

        }


        public tk.JsonTcpClient GetClient()
        {
            return TcpClient;
        }

        void OnProtocolVersion(JSONObject msg)
        {
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("msg_type", "protocol_version");
            json.AddField("version", "2");

            TcpClient.SendMsg(json);
        }
        void SendSubLoaded()
        {
            if (TcpClient == null)
                return;

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("msg_type", "sub_loaded");
            TcpClient.SendMsg(json);
            Debug.Log("sub loaded.");
        }

        void OnResetSubRecv(JSONObject json)
        {
            bResetSub = true;
        }

        void OnControlsRecv(JSONObject json)
        {
            try
            {
                float upForce = float.Parse(json["up_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float lateralForce =float.Parse(json["lateral_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float forwardForce = float.Parse(json["forward_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float rollForce = float.Parse(json["roll_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float pitchForce = float.Parse(json["pitch_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float yawForce = float.Parse(json["yaw_force"].str, CultureInfo.InvariantCulture.NumberFormat);

                sub.upForce = upForce;
                sub.lateralForce = lateralForce;
                sub.forwardForce = forwardForce;
                sub.rollForce = rollForce;
                sub.pitchForce = pitchForce;
                sub.yawForce = yawForce;

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }


        void SendTelemetry()
        {
            if (UdpClient == null)
                return;

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("msg_type", "telemetry");

            foreach (ISensor sensor in sensors)
            {
                sensor.RequestObs(json);
            }

            Debug.Log("sending telemetry");
            UdpClient.SendMsg(json);

        }

        void Update()
        {
            if (bResetSub)
            {
                sub.RestorePosRot();
                bResetSub = false;
            }

            timeSinceLastCapture += Time.deltaTime;
            if (timeSinceLastCapture > 1.0f / transferRate)
            {
                timeSinceLastCapture -= (1.0f / transferRate);
                SendTelemetry();
            }

        }

    }
}
