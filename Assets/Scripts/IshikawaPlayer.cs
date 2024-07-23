using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IshikawaPlayer : MonoBehaviour
{
    [SerializeField] float speed; //移動速度



    void Start()
    {
        speed = 5f; //移動速度の初期化
    }



    void Update()
    {
        //前方移動
        if (Input.GetKey(KeyCode.W)) transform.position += speed * transform.forward * Time.deltaTime;
 
        //後方移動
        if (Input.GetKey(KeyCode.S)) transform.position -= speed * transform.forward * Time.deltaTime;
 
        //右移動
        if (Input.GetKey(KeyCode.D)) transform.position += speed * transform.right * Time.deltaTime;
 
        //左移動
        if (Input.GetKey(KeyCode.A)) transform.position -= speed * transform.right * Time.deltaTime;
    }
}
