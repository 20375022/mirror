using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerUI : MonoBehaviour
{
    public GameObject MainCamera;

    // ホストボタン押下時に呼ばれる
    public void OnHostButton()
    {
        GetComponent<NetworkManager>().StartHost();
        MainCamera.gameObject.SetActive(false);
    }

    // クライアントボタン押下時に呼ばれる
    public void OnClientButton()
    {
        GetComponent<NetworkManager>().networkAddress = "172.19.8.169"; // IP指定
//        GetComponent<NetworkManager>().networkAddress = "172.19.4.64"; // IP指定
//        GetComponent<NetworkManager>().networkAddress = "192.168.0.6"; // IP指定
        GetComponent<NetworkManager>().StartClient();
        MainCamera.gameObject.SetActive(false);
    }

    // セーバーボタン押下時に呼ばれる
    public void OnServerButton()
    {
        GetComponent<NetworkManager>().StartServer();
        MainCamera.gameObject.SetActive(false);
    }
}

