using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class NetworkUtils : NetworkBehaviour
{
    public string ipv4;
    public GameObject ip_obj;
    Text ip_text;
    GameObject prefab;

    void Start() 
    {
//        ip_text = Instantiate(ip_obj);
        ip_text = ip_obj.GetComponentInChildren<Text>();

        ipv4 = IPManager.GetIP(ADDRESSFAM.IPv4);
    }

    void Update()
    {
        if (ipv4 != null)
        {
            ip_text.text = string.Format("{000:000:000:000}", ipv4);
        }
    }
    
    //ゲーム開始ボタンを押したあと
    //IPアドレス表示非表示
}