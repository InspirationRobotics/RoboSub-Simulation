using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace tk
{
    [RequireComponent(typeof(UdpClient))]

    public class JsonUdpClient : MonoBehaviour
    {
        public string IP = "127.0.0.1";
        public int[] ports = new int[] { 9093, 9094 };

        const string packetTerminationChar = "\n";

        IPEndPoint remoteEndPoint;
        UdpClient client = new UdpClient();

        public void SendMsg(JSONObject msg)
        {
            string packet = msg.ToString() + packetTerminationChar;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(packet);

            foreach (int port in ports)
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
                client.Send(data, data.Length, remoteEndPoint);
            }
        }

    }
}


