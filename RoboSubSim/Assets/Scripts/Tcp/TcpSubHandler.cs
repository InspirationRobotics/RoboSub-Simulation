using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace tk
{
    public class TcpSubHandler : MonoBehaviour
    {
        public GameObject subObj;
        Submarine sub;
        public GameObject[] sensorsGO;
        public ISensor[] sensors;
        private tk.JsonTcpClient client;

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

        public void Init(tk.JsonTcpClient _client)
        {
            client = _client;

            if (client == null)
                return;

            client.dispatchInMainThread = false; //too slow to wait.
            client.dispatcher.Register("get_protocol_version", new tk.Delegates.OnMsgRecv(OnProtocolVersion));
            client.dispatcher.Register("control", new tk.Delegates.OnMsgRecv(OnControlsRecv));
            client.dispatcher.Register("reset_sub", new tk.Delegates.OnMsgRecv(OnResetSubRecv));
            // client.dispatcher.Register("set_position", new tk.Delegates.OnMsgRecv(OnSetPosition));
        }


        public tk.JsonTcpClient GetClient()
        {
            return client;
        }

        void OnProtocolVersion(JSONObject msg)
        {
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("msg_type", "protocol_version");
            json.AddField("version", "2");

            client.SendMsg(json);
        }
        void SendSubLoaded()
        {
            if (client == null)
                return;

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("msg_type", "sub_loaded");
            client.SendMsg(json);
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
                float forwardForce = float.Parse(json["forward_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float rollForce = float.Parse(json["roll_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float pitchForce = float.Parse(json["pitch_force"].str, CultureInfo.InvariantCulture.NumberFormat);
                float yawForce = float.Parse(json["yaw_force"].str, CultureInfo.InvariantCulture.NumberFormat);

                sub.upForce = upForce;
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

            if (client == null)
                return;

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("msg_type", "telemetry");

            foreach (ISensor sensor in sensors)
            {
                sensor.RequestObs(json);
            }

            client.SendMsg(json);

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
