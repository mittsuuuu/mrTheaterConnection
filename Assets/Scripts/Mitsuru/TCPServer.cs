using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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

    private string receiveMessage = string.Empty;

    // テスト用
    private string sendMessage    = "Connection is OK";

    /// <summary>
    /// 初期化
    /// </summary>

    private void Awake()
    {
        Debug.Log(gameObject.transform.position);

        var ipAddress = IPAddress.Parse(server_ip);
        tcpListener = new TcpListener(ipAddress, server_port);
        tcpListener.Start();

        userDb = connectionManager.GetComponent<userDB>();
        udpServer = gameObject.GetComponent<UDPServer>();

        Task.Run(() => Wait());
    }

    private void OnProcess(TcpClient client)
    {
        var client_data = client.Client;
        IPEndPoint re = (IPEndPoint)client_data.RemoteEndPoint; // クライアントのデータを格納する変数

        userDb.registerData(re.Address.ToString(), re.Port);
        Debug.Log("接続完了 : " + re.AddressFamily + ", " + re.Address + ", " + re.Port) ; // クライアントのIPとポートの表示

        networkStream = client.GetStream();

        while (true)
        {
            var buffer = new byte[256];
            var count = networkStream.Read(buffer, 0, buffer.Length);

            // クライアントからの接続が切断された場合
            if (count == 0)
            {
                Debug.Log("切断");

                client?.Dispose();
                OnDestroy();

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
            Debug.Log(networkStream);
            try
            {
                var buffer = Encoding.UTF8.GetBytes(sendMessage);

                if (networkStream != null)
                {
                    networkStream.Write(buffer, 0, buffer.Length);
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
    }

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
