using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace tk
{
    [RequireComponent(typeof(UdpClient))]

    public class JsonUdpClient : MonoBehaviour
    {
        public string IP = "127.0.0.1";
        public int[] ports = new int[] { 9093, 9094 };

        const string packetTerminationChar = "\n";
        const int maxDgramSize = 65536 - 64;

        IPEndPoint remoteEndPoint;
        UdpClient client = new UdpClient();


        public void SendMsg(JSONObject msg)
        {
            string packet = msg.ToString() + packetTerminationChar;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(packet);

            foreach (int port in ports)
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

                int numSegments = Mathf.CeilToInt((float)data.Length / (float)maxDgramSize);
                // if the packet is too large, split it into smaller segments
                for (int i = 0; i < numSegments; i++)
                {
                    byte[] sliced_data = (data.Skip(i * maxDgramSize).Take(maxDgramSize)).ToArray();
                    client.Client.SendTo(sliced_data, remoteEndPoint);
                }


            }
        }

    }
}


