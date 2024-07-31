using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userDB : MonoBehaviour
{
    string _ip;
    int _port;
    int _id;

    // DBにデータを登録する用のメソッド
    public void RegisterData(string ip, int port)
    {
        _ip = ip;
        _port = port;
    }
}