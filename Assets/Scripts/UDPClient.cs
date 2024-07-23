using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ClientData //接続しているクライアントPCのデータを保存する
{
    int IP;
    int Port;
    int Mac;
    int ID;

    public ClientData(int i, int p, int m)
    {
        IP = i;
        Port = p;
        Mac = m;
    }

    public void getID(int id)
    {
        ID = id;
    }
}


public class UDPClient : MonoBehaviour
{
    List<ClientData> datas;

    void Start()
    {
        datas = new List<ClientData>();
    }

    void setID()
    {
        datas[0].getID(datas.Count);
    }
}
