using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace tk
{
    public class TcpSubHandler : MonoBehaviour
    {
        public GameObject subObj;
        public Submarine sub;
        public ISensor[] sensors;
        private tk.JsonTcpClient client;

        float timeSinceLastCapture = 0.0f;
        float transferRate = 20.0f;
        bool bResetSub = false;


        void Awake()
        {
            sub = subObj.GetComponent<Submarine>();

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
                // ai_steering = float.Parse(json["steering"].str, CultureInfo.InvariantCulture.NumberFormat);
                // ai_throttle = float.Parse(json["throttle"].str, CultureInfo.InvariantCulture.NumberFormat);
                // ai_brake = float.Parse(json["brake"].str, CultureInfo.InvariantCulture.NumberFormat);

                // ai_steering = clamp(ai_steering, -1.0f, 1.0f);
                // ai_throttle = clamp(ai_throttle, -1.0f, 1.0f);
                // ai_brake = clamp(ai_brake, 0.0f, 1.0f);

                // ai_steering *= steer_to_angle;

                // car.RequestSteering(ai_steering);
                // car.RequestThrottle(ai_throttle);
                // car.RequestFootBrake(ai_brake);
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
