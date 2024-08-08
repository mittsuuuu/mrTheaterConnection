using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

public class UDPClient : MonoBehaviour
{
    [SerializeField] GameObject avater;
    TCPClient tcpClient;

    UdpClient udpClient;

    Transform tf;

    string ip;
    int port;
    int id;

    private void Start()
    {
        tcpClient = gameObject.GetComponent<TCPClient>();
        ip = tcpClient.server_ip;
        port = tcpClient.server_port;

        tf = avater.GetComponent<Transform>();

        Debug.Log("UDP : " + ip + ", " + port);
        udpClient = new UdpClient(ip, port);
        Debug.Log("UDPClient : " + (IPEndPoint)udpClient.Client.RemoteEndPoint);
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
            string message = tf.position.x.ToString();
            Debug.Log(tf);
            //udpClient.Send(message, message.Length);
            Debug.Log("UDP Sent" + message);
        }
    }

    private void OnDestroy()
    {
        udpClient.Dispose();
    }
}
