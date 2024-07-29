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
    public string server_ip = "";
    public int server_port = 9000;

    private TcpListener tcpListener;
    private TcpClient tcpClient;
    private NetworkStream networkStream;

    private string receiveMessage = string.Empty;

    /// <summary>
    /// 初期化
    /// </summary>

    private void Awake()
    {
        Task.Run(() => OnProcess());
    }

    private void OnProcess()
    {
        var ipAddress = IPAddress.Parse(server_ip);
        tcpListener = new TcpListener(ipAddress, server_port);
        tcpListener.Start();
        Debug.Log("待機中");

        // クライアントからの接続を待機する
        tcpClient = tcpListener.AcceptTcpClient();
        var client_data = tcpClient.Client;

        Debug.Log("接続完了 : " + client_data);

        networkStream = tcpClient.GetStream();

        while (true)
        {
            var buffer = new byte[256];
            var count = networkStream.Read(buffer, 0, buffer.Length);

            // クライアントからの接続が切断された場合
            if (count == 0)
            {
                Debug.Log("切断");

                OnDestroy();

                Task.Run(() => OnProcess());

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
        GUILayout.TextArea(receiveMessage);
    }

    private void OnDestroy()
    {
        networkStream?.Dispose();
        tcpClient?.Dispose();
        tcpListener?.Stop();
    }
}
