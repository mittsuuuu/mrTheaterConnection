using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokyoPlayer : MonoBehaviour
{
    [SerializeField] float speed; //移動速度



    void Start()
    {
        speed = 5f; //移動速度の初期化
    }



    void Update()
    {
        //前方移動
        if (Input.GetKey(KeyCode.I)) transform.position += speed * transform.forward * Time.deltaTime;
 
        //後方移動
        if (Input.GetKey(KeyCode.K)) transform.position -= speed * transform.forward * Time.deltaTime;
 
        //右移動
        if (Input.GetKey(KeyCode.L)) transform.position += speed * transform.right * Time.deltaTime;
 
        //左移動
        if (Input.GetKey(KeyCode.J)) transform.position -= speed * transform.right * Time.deltaTime;
    }
}
