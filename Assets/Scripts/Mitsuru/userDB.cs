using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userDB : MonoBehaviour
{
    User user;
    UDPServer udpServer;

    List<User> usersDB;
    [SerializeField] List<GameObject> models;

    /// <summary>
    /// DBの要素数を返す
    /// </summary>
    public int NUM
    {
        get { return usersDB.Count; }
    }

    private void Start()
    {
        usersDB = new List<User>();
    }

    private void Update()
    {
        for(int i = 0; i < usersDB.Count; i++)
        {
            models[i].transform.position = usersDB[i].POS;
            models[i].transform.rotation = usersDB[i].RO;
        }
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
    }

    public void removeData(int id)
    {
        foreach (User u in usersDB)
        {
            if (u.ID == id)
            {
                usersDB.Remove(u);
                break;
            }
        }
    }

    /// <summary>
    /// データベースにTransformの情報を渡し，モデルに反映させる
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos"></param>
    /// <param name="ro"></param>
    public void setTransform(int id,  Vector3 pos, Quaternion ro)
    {
        usersDB[id].setTransform(pos, ro);
        //models[id].transform.position = pos;
        //models[id].transform.rotation = ro;
    }

    /// <summary>
    /// IPアドレスが一致するIDをDBから探し返す
    /// ない場合は-1を返す
    /// </summary>
    /// <param name="addr"></param>
    /// <returns></returns>
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
    //public Transform getData(int id)
    //{
    //    return usersDB[id].TF;
    //}
}