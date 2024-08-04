using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPServer : MonoBehaviour
{
    userDB userDb;

    [SerializeField] GameObject connectionManager;

    List<UdpClient> udpClients;

    void Start()
    {
        userDb = connectionManager.GetComponent<userDB>();
        udpClients = new List<UdpClient>();
    }

    public void addClient(string addr, int port)
    {
        udpClients.Add(new UdpClient(addr, port));

        //Task.Run(() => SendData(udpClients[udpClients.Count-1], addr));
        SendData(udpClients[udpClients.Count - 1], addr);

        udpClients[udpClients.Count - 1].BeginReceive(ReceiveDatas, udpClients[udpClients.Count - 1]);
    }

    // クライアントが増えた時にクライアントが保持する情報を渡す
    private void SendData(UdpClient client, string addr)
    {
        int id = userDb.getId(addr);
        Transform tf = userDb.getData(id);
        var message = Encoding.UTF8.GetBytes(tf.ToString());
        client.Send(message, message.Length) ;
    }

    // データを受け取るメソッド
    private void ReceiveDatas(System.IAsyncResult result)
    {
        int myId;
        float[] transforms = new float[7];

        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        var receiveMessage = Encoding.UTF8.GetString(getByte);
        receiveMessage = receiveMessage.Trim();

        string[] messages = receiveMessage.Split(',');

        // id, pos_x, pos_y, pos_z, ro_x, ro_y, ro_z
        if(messages.Length == 7)
        {
            myId = int.Parse(messages[0]);
            for (int i = 1; i < 8; i++) transforms[i-1] = float.Parse(messages[i]);
        }

        Debug.Log(getByte);
    }

    private void OnDestroy()
    {
        foreach (UdpClient uc in udpClients) uc.Dispose();
        
    }
}
