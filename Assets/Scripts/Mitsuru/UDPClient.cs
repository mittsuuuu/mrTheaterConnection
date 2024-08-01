using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

public class UDPClient : MonoBehaviour
{
    TCPServer tcpServer;

    UdpClient udpClient;

    string ip;
    int port;
    int id;

    private void Start()
    {
        ip = tcpServer.server_ip;
        port = tcpServer.server_port;

        udpClient = new UdpClient(ip, port);
    }

    public void setId(int id)
    {
        this.id = id;

        Task.Run(() => SendData());
    }

    void SendData()
    {
        while(true)
        {
            var message = Encoding.UTF8.GetBytes(id.ToString() + "," + transform.ToString());
            udpClient.Send(message, message.Length);
        }
    }

    private void OnDestroy()
    {
        udpClient.Dispose();
    }
}
