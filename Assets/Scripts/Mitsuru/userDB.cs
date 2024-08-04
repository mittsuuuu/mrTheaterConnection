using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userDB : MonoBehaviour
{
    User user;
    UDPServer udpServer;

    List<User> usersDB;
    [SerializeField] List<GameObject> models;

    private void Start()
    {
        usersDB = new List<User>();
    }

    /// <summary>
    /// DBにデータを最初に登録する用のメソッド
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void registerData(string ip, int port)
    {
        int id = usersDB.Count;
        user = new User(ip, port, id);
        usersDB.Add(user);

        //foreach(User u in usersDB)
        //{
        //    Debug.Log(u.IP + ", " + u.PORT + ", " + u.ID);
        //}
    }

    public void setTransform(int id, Transform rtf)
    {
        usersDB[id].setTransform(rtf);
    }

    public int getId(string addr)
    {
        foreach(User u in usersDB)
        {
            if(u.IP.Equals(addr))
            {
                return u.ID;
            }
        }
        return -1;
    }
    public Transform getData(int id)
    {
        return usersDB[id].TF;
    }
}