using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class UDPClient : MonoBehaviour
{
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    CancellationToken ct;

    [SerializeField] GameObject avater;
    TCPClient tcpClient;

    UdpClient udpClient;

    static Transform tf;


    StringBuilder message_sb;

    string ip;
    byte[] message;
    int port;
    int id;

    bool connection = true;
    bool init = false;

    private void Start()
    {
        ct = tokenSource.Token;

        tcpClient = gameObject.GetComponent<TCPClient>();
        ip = tcpClient.server_ip;
        port = tcpClient.server_port;

        tf = avater.GetComponent<Transform>();

        Debug.Log("UDP : " + ip + ", " + port);
        udpClient = new UdpClient(ip, port);
        Debug.Log("UDPClient : " + (IPEndPoint)udpClient.Client.RemoteEndPoint);

        message_sb = new StringBuilder(TransformToString(tf));
        init = true;
        Debug.Log("init is true");
    }

    private void Update()
    {
        message_sb.Clear();
        message_sb.Append(TransformToString(tf));
    }

    public void setId(int id)
    {
        this.id = id;

        while(!init) { }

        Task.Run(() => SendData(), tokenSource.Token);
        tokenSource.Cancel();
    }

    void SendData()
    {
        Debug.Log("Thread start");
        while(connection)
        {
            try
            {
                message = Encoding.UTF8.GetBytes(message_sb.ToString());
                udpClient.Send(message, message.Length);
                //Debug.Log("UDP Sent" + message);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.Log("Task Cancelled");
                ct.ThrowIfCancellationRequested();
            }
        }
    }

    private string TransformToString(Transform tf)
    {
        string message_str;

        message_str = id.ToString() + "," + tf.position.x.ToString() + "," + tf.position.y.ToString() + "," + tf.position.z.ToString() + "," +
                        tf.rotation.x.ToString() + "," + tf.rotation.y.ToString() + "," + tf.rotation.z.ToString() + "," + tf.rotation.w.ToString();
        return message_str;
    }

    private void OnDestroy()
    {
        connection = false;
        udpClient.Dispose();
        tokenSource.Dispose();
    }
}
