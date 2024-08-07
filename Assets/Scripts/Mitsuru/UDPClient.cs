using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

public class UDPClient : MonoBehaviour
{
    TCPClient tcpClient;

    UdpClient udpClient;

    string ip;
    int port;
    int id;

    private void Start()
    {
        tcpClient = gameObject.GetComponent<TCPClient>();
        ip = tcpClient.server_ip;
        port = tcpClient.server_port;

        Debug.Log("UDP : " + ip + ", " + port);
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
            Debug.Log("sending");
            var message = Encoding.UTF8.GetBytes(id.ToString() + "," + transform.ToString());
            udpClient.Send(message, message.Length);
            Debug.Log("UDP Sent" + message);
        }
    }

    private void OnDestroy()
    {
        udpClient.Dispose();
    }
}
