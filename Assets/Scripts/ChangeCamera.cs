using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] GameObject ishikawaCamera; //石川県カメラ格納用
    [SerializeField] GameObject vertualCamera; //バーチャル内カメラ格納用
    [SerializeField] GameObject tokyoCamera; //東京都カメラ格納用



    void Start ()
    {
        //起動するカメラの初期化
        ishikawaCamera.SetActive(false);
        vertualCamera.SetActive(true);
        tokyoCamera.SetActive(false);
	}



	void Update () {
        if(Input.GetKey("1")) //1が押されたら石川県カメラをアクティブにする
        {
            ishikawaCamera.SetActive(true);
            vertualCamera.SetActive(false);
            tokyoCamera.SetActive(false);
        }
        else if(Input.GetKey("2")) //2が押されたらバーチャル内カメラをアクティブにする
        {
            ishikawaCamera.SetActive(false);
            vertualCamera.SetActive(true);
            tokyoCamera.SetActive(false);
        }
        else if(Input.GetKey("3")) //3が押されたら東京都カメラをアクティブにする
        {
            ishikawaCamera.SetActive(false);
            vertualCamera.SetActive(false);
            tokyoCamera.SetActive(true);
        }
	}
}
