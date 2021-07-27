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
        public int localPort = 9093; // the port you want your sim to send the packets from
        public int[] remotePorts = new int[] { 9094, 9095 }; // the port you want your sim to send the packets to

        const string packetTerminationChar = "\n";
        const int maxDgramSize = 65536 - 64;

        IPEndPoint localEndPoint;
        IPEndPoint[] remoteEndPoints;
        UdpClient client;

        public void Init()
        {
            remoteEndPoints = new IPEndPoint[remotePorts.Length];

            for (int i = 0; i < remotePorts.Length; i++)
            {
                remoteEndPoints[i] = new IPEndPoint(IPAddress.Parse(ip), remotePorts[i]);
            }

            localEndPoint = new IPEndPoint(IPAddress.Any, localPort);

            client = new UdpClient();
            client.Client.Bind(localEndPoint);
        }

        public void SendMsg(JSONObject msg)
        {
            if (_tcpClient == null) { return; }

            string packet = msg.ToString() + packetTerminationChar;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(packet);

            if (ip == null)
            {
                ip = ((IPEndPoint)(_tcpClient.client._clientSocket.RemoteEndPoint)).Address.ToString();
                if (client == null)
                {
                    Init();
                }
                return;
            }


            foreach (IPEndPoint remoteEndPoint in remoteEndPoints)
            {

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


