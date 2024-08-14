using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TCP 通信を行うサーバ側のスクリプト
/// </summary>

public class TCPServer : MonoBehaviour
{
    userDB userDb;
    UDPServer udpServer;

    [SerializeField] GameObject connectionManager;

    public string server_ip = "10.140.2.163";
    public int server_port = 9000;

    private TcpListener tcpListener;
    private NetworkStream networkStream;

    private List<NetworkStream> networkStreams;

    private string receiveMessage = string.Empty;

    // テスト用
    private string sendMessage    = "Connection is OK";

    // Taskを停止するようのトークン
    private List<CancellationTokenSource> cancellTokens;

    /// <summary>
    /// 初期化
    /// </summary>

    private void Awake()
    {
        Debug.Log(gameObject.transform.position);

        var ipAddress = IPAddress.Parse(server_ip);
        tcpListener = new TcpListener(ipAddress, server_port);
        tcpListener.Start();
        networkStreams = new List<NetworkStream>();

        userDb = connectionManager.GetComponent<userDB>();
        udpServer = gameObject.GetComponent<UDPServer>();

        Task.Run(() => Wait());
    }

    private void OnProcess(TcpClient client)
    {
        int thisID;

        Socket client_data = client.Client;
        IPEndPoint re = (IPEndPoint)client_data.RemoteEndPoint; // クライアントのデータを格納する変数

        userDb.registerData(re.Address.ToString(), re.Port);
        udpServer.addClient();
        Debug.Log("接続完了 : " + re.AddressFamily + ", " + re.Address + ", " + re.Port) ; // クライアントのIPとポートの表示

        networkStream = client.GetStream();
        networkStreams.Add(networkStream);
        var net = networkStreams[networkStreams.Count - 1];

        thisID = userDb.NUM - 1;

        net.Write(Encoding.UTF8.GetBytes("ID,"+thisID.ToString()));
        
        while (true)
        {
            var buffer = new byte[256];
            var count = net.Read(buffer, 0, buffer.Length);

            // クライアントからの接続が切断された場合
            if (count == 0)
            {
                Debug.Log("切断");
                userDb.removeData(thisID);

                client?.Dispose();
                net?.Dispose();

                break;
            }

            // データを受信した時
            var message = Encoding.UTF8.GetString(buffer, 0, count);
            receiveMessage += message + "\n";

            Debug.LogFormat("受信成功：{0}", message);
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("送信返し"))
        {
            Debug.Log(networkStreams);
            try
            {
                var buffer = Encoding.UTF8.GetBytes(sendMessage);

                if (networkStream != null)
                {
                    //networkStream.Write(buffer, 0, buffer.Length);
                    foreach (NetworkStream net in networkStreams) net.Write(buffer, 0, buffer.Length);
                    Debug.Log("送信成功");
                }
                else
                {
                    Debug.Log("null");
                }
            }
            catch (Exception)
            {
                Debug.LogError("送信失敗");
            }
        }
        GUILayout.TextArea(receiveMessage);
    }

    private void OnDestroy()
    {
        networkStream?.Dispose();
        tcpListener?.Stop();

        foreach (NetworkStream net in networkStreams) net?.Dispose();
    }

    /// <summary>
    /// 接続を待つメソッド
    /// 新しい接続があったらスレッドを立てる
    /// </summary>
    private void Wait()
    {
        Debug.Log("待機中");

        while(true)
        {
            TcpClient client = tcpListener.AcceptTcpClient();

            Task.Run(() => OnProcess(client));
        }
    }
}
