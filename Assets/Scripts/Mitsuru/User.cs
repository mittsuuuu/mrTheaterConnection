using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    string _ip;
    int _port;
    int _id;

    Vector3 _pos;
    Quaternion _ro;

    public User(string ip, int port, int id)
    {
        _ip = ip;
        _port = port;
        _id = id;
        _pos = Vector3.zero;
        _ro = new Quaternion(0, 0, 0, 0);
    }

    public void setTransform(Vector3 pos, Quaternion ro)
    {
        _pos = pos;
        _ro = ro;
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
}
