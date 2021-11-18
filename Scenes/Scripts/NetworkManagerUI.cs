using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerUI : MonoBehaviour
{
    public Canvas canvas;

    // ホストボタン押下時に呼ばれる
    public void OnHostButton()
    {
        GetComponent<NetworkManager>().StartHost();
        canvas.gameObject.SetActive(false);
    }

    // クライアントボタン押下時に呼ばれる
    public void OnClientButton()
    {
        GetComponent<NetworkManager>().networkAddress = "172.19.8.146"; // IP指定
        GetComponent<NetworkManager>().StartClient();
        canvas.gameObject.SetActive(false);
    }

    // セーバーボタン押下時に呼ばれる
    public void OnServerButton()
    {
        GetComponent<NetworkManager>().StartServer();
        canvas.gameObject.SetActive(false);
    }
}