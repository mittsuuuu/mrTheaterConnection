using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userDB : MonoBehaviour
{
    User user;

    List<User> usersDB;

    private void Start()
    {
        usersDB = new List<User>();
    }

    // DBにデータを登録する用のメソッド
    public void registerData(string ip, int port)
    {
        int id = usersDB.Count;
        user = new User(ip, port, id);
        usersDB.Add(user);

        foreach(User u in usersDB)
        {
            //Debug.Log(u.IP + ", " + u.PORT + ", " + u.ID);
        }
    }
}