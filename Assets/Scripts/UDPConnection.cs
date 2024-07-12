using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Net.Sockets;
using System.Threading;


public class UDPConnection : MonoBehaviour
{
    [SerializeField] Toggle flagClient;
    userDB udb;

    Thread thread;

    static UdpClient udp;
    static UdpClient udpSend;

    const string SERVER_IP = "10.30.114.33";
    string client_ip;

    const int SERVER_PORT = 9000;
    const int CLIENT_PORT = 9001;

    void Start()
    {
        if(!flagClient)
        {
            
        }
    }

    void Update()
    {

    }

    // 最初の通信用
    // 通信を確立したときにIPアドレスとポート番号を受けとる
    void firestConnect()
    {
        
    }

    void clientInit()
    {
        udpSend = new UdpClient();
        udpSend.Connect(SERVER_IP, SERVER_PORT);
    }
    void serverInit()
    {
        udp = new UdpClient(SERVER_PORT); //受信用のポートを設定
    }
}