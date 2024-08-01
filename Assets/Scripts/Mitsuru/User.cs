using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    string _ip;
    int _port;
    int _id;
    Transform? _tf;

    public User(string ip, int port, int id)
    {
        _ip = ip;
        _port = port;
        _id = id;
        _tf = null;
    }

    public void setTransform(Transform tf)
    {
        _tf = tf;
    }

    public string IP
    {
        get { return _ip; }
    }
    public int PORT
    {
        get { return _port; }
    }
    public int ID
    {
        get { return _id; }
    }
    public Transform TF
    {
        get => _tf;
    }
}
