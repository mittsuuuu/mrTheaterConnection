using System;
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

    UdpClient client;
    
    List<CancellationTokenSource> tokenSources;
    List<CancellationToken> cts;

    Thread thread;

    void Start()
    {
        userDb = connectionManager.GetComponent<userDB>();
        tokenSources = new List<CancellationTokenSource>();
        cts = new List<CancellationToken>();

        client = new UdpClient(9000);
    }

    /// <summary>
    /// クライアントが追加されたときに呼ばれるメソッド
    /// トークンの追加と新しいスレッドの作成をする
    /// </summary>
    public void addClient()
    {
        Debug.Log("add client udp");
        try
        {
            tokenSources.Add(new CancellationTokenSource());
            cts.Add(tokenSources[tokenSources.Count - 1].Token);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        

        Task.Run(() => ReceiveDatas( cts.Count-1), cts[cts.Count-1]);
    }


    // データを受け取るメソッド
    private void ReceiveDatas( int ctId)
    {
        bool connection = true;
        Debug.Log("Receiving");
        int myId = -1;
        Vector3 pos = new Vector3(0, 0, 0);
        Quaternion ro = new Quaternion(0, 0, 0, 0);

        float[] transforms = new float[7];

        //Debug.Log("UDP receiving by " + client.Client.RemoteEndPoint);
        IPEndPoint ipEnd = null;

        while (connection)
        {            
            byte[] getByte = client.Receive(ref ipEnd);

            var receiveMessage = Encoding.UTF8.GetString(getByte);
            receiveMessage = receiveMessage.Trim();

            string[] messages = receiveMessage.Split(',');

            //Debug.Log("getByte : " + messages.Length);

            ///< summary>
            /// 含まれる引数：id, pos_x, pos_y, pos_z, ro_x, ro_y, ro_z, ro_w
            /// </summary>
            if (messages.Length >= 8)
            {
                myId = int.Parse(messages[0]);
                for (int i = 1; i < 8; i++) transforms[i - 1] = float.Parse(messages[i]);
            }
            //Debug.Log("convert string to float");
            pos.Set(transforms[0], transforms[1], transforms[2]);
            ro.Set(transforms[3], transforms[4], transforms[5], transforms[6]);

            try
            {
                userDb.setTransform(myId, pos, ro);
            }
            catch
            {
                connection = false;
                //Debug.LogError(myId + e.ToString());
            }
            //Debug.Log("Received : " + receiveMessage);
        }
        Debug.Log("UDP終わり");

        cts[ctId].ThrowIfCancellationRequested();
    }

    private void OnDestroy()
    {
        client.Dispose();
        foreach (CancellationTokenSource t in tokenSources) t.Dispose();
    }
}
