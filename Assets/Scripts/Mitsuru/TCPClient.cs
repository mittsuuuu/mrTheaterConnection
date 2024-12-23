using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 初回にTCP通信を行うクライアント側のスクリプト
/// </summary>
public class TCPClient : MonoBehaviour
{
    UDPClient udpClient;

    public string server_ip = "";
    public int server_port = 9000;

    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private bool isConnection;

    private String testMessage = "TCPによる送信";

    CancellationToken ct; //タスク破棄用のトークン

    private void Awake()
    {
        var tokenSource = new CancellationTokenSource();
        ct = tokenSource.Token;

        udpClient = gameObject.GetComponent<UDPClient>();

        WaitConnection();

        Task.Run(() => Listen(), ct);
        tokenSource.Dispose();
    }

    /// <summary>
    /// GUI を描画する時に呼び出される
    /// </summary>

    public void OnGUI()
    {
        if (!isConnection)
        {
            GUILayout.Label("接続していません");
            return;
        }

        // サーバに送信する文字列
        testMessage = GUILayout.TextField(testMessage);

        if (GUILayout.Button("送信"))
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(testMessage);
                networkStream.Write(buffer, 0, buffer.Length);

                Debug.LogFormat("送信成功：{0}", testMessage);
            }
            catch (Exception)
            {
                Debug.LogError("送信失敗");
            }
        }
    }

    /// <summary>
    /// 破棄する時に呼び出される
    /// </summary>
    private void OnDestroy()
    {
        // インスタンスの生成に失敗している可能性もあるので
        // null 条件演算子を使用している
        tcpClient?.Dispose();
        networkStream?.Dispose();

        Debug.Log("切断");
    }

    private void Listen()
    {
        while (true)
        {
            var buffer = new byte[256];
            var count = networkStream.Read(buffer, 0, buffer.Length);
            var message = Encoding.UTF8.GetString(buffer, 0, count);
            Debug.LogFormat("受信したメッセージ：{0}", message);
            String[] text = message.Split(",");
            if (text[0].Equals("ID"))
            {
                Debug.Log("set id" + text[1]);
                udpClient.setId(int.Parse(text[1]));
                ct.ThrowIfCancellationRequested();
            }
        }
    }

    private void WaitConnection()
    {
        try
        {
            tcpClient = new TcpClient(server_ip, server_port);
            networkStream = tcpClient.GetStream();
            isConnection = true;

            Debug.Log(gameObject.name + " 接続成功");
        }
        catch (SocketException)
        {
            Debug.LogError("接続失敗");
        }
    }
}