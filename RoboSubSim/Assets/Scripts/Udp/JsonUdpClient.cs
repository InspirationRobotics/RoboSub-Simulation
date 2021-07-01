using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace tk
{
    [RequireComponent(typeof(UdpClient))]

    public class JsonUdpClient : MonoBehaviour
    {
        private string ip;
        public tk.JsonTcpClient _tcpClient;
        public int[] ports = new int[] { 9093, 9094 };

        const string packetTerminationChar = "\n";
        const int maxDgramSize = 65536 - 64;

        IPEndPoint remoteEndPoint;
        UdpClient client = new UdpClient();

        public void SendMsg(JSONObject msg)
        {
            if (_tcpClient == null) { return; }

            string packet = msg.ToString() + packetTerminationChar;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(packet);

            if (ip == null)
                ip = ((IPEndPoint)(_tcpClient.client._clientSocket.RemoteEndPoint)).Address.ToString();

            foreach (int port in ports)
            {
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

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


