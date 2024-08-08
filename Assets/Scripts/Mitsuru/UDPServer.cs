using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPServer : MonoBehaviour
{
    userDB userDb;

    [SerializeField] GameObject connectionManager;

    List<UdpClient> udpClients;

    Thread thread;

    void Start()
    {
        userDb = connectionManager.GetComponent<userDB>();
        udpClients = new List<UdpClient>();
    }

    public void addClient(string addr, int port)
    {
        Debug.Log("add client udp");
        udpClients.Add(new UdpClient(addr, port));

        Task.Run(() => ReceiveDatas(udpClients[udpClients.Count-1]));
        //Task.Run(() => SendData(udpClients[udpClients.Count-1], addr));
        //SendData(udpClients[udpClients.Count - 1], addr);

       // udpClients[udpClients.Count - 1].BeginReceive(ReceiveDatas, udpClients[udpClients.Count - 1]);
    }

    // クライアントが増えた時にクライアントが保持する情報を渡す
    //private void SendData(UdpClient client, string addr)
    //{
    //    int id = userDb.getId(addr);
    //    Transform tf = userDb.getData(id);
    //    var message = Encoding.UTF8.GetBytes(tf.ToString());
    //    client.Send(message, message.Length) ;
    //}

    // データを受け取るメソッド
    private void ReceiveDatas(UdpClient client)
    {
        int myId = -1;
        Vector3 pos;
        Quaternion ro;

        float[] transforms = new float[7];

        while (true)
        {
            Debug.Log("UDP receiving by " + client.Client);
            IPEndPoint ipEnd = null;

            byte[] getByte = client.Receive(ref ipEnd);

            var receiveMessage = Encoding.UTF8.GetString(getByte);
            receiveMessage = receiveMessage.Trim();

            string[] messages = receiveMessage.Split(',');

            // id, pos_x, pos_y, pos_z, ro_x, ro_y, ro_z, ro_w
            if (messages.Length >= 8)
            {
                myId = int.Parse(messages[0]);
                for (int i = 1; i < 8; i++) transforms[i - 1] = float.Parse(messages[i]);
            }
            pos = new Vector3(transforms[0], transforms[1], transforms[2]);
            ro = new Quaternion(transforms[3], transforms[4], transforms[5], transforms[6]);

            userDb.setTransform(myId, pos, ro);

            Debug.Log(getByte);
        }        
    }

    private void OnDestroy()
    {
        foreach (UdpClient uc in udpClients) uc.Dispose();
        
    }
}
