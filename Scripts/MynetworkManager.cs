using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class MynetworkManager : NetworkManager
{
    //キャラクター
    public GameObject piesu;        // ピエロと素体
    public GameObject pierun;       // ピエロとランナー
    public GameObject killsu;       // ジェーソンと素体
    public GameObject killrun;      // ジェーソンとランナー

    static int gamemode = 0;

    //====================
    // ホスト・クライアント・サーバーの開始・停止
    //====================
    // ホストの開始時に呼ばれる
    public override void OnStartHost()
    {
        base.OnStartHost();
        print("OnStartHost");
    }
    // クライアントの開始時に呼ばれる
    public override void OnStartClient()
    {
        base.OnStartClient();
        print("OnStartClient");
    }
    // サーバーの開始時に呼ばれる
    public override void OnStartServer()
    {
        base.OnStartServer();
        print("OnStartServer");
        NetworkServer.RegisterHandler<CreateMMOCharacterMessage>(OnCreateCharacter);
    }
    // ホストの停止時に呼ばれる
    public override void OnStopHost()
    {
        base.OnStopHost();
        print("OnStopHost");
    }
    // クライアントの停止時に呼ばれる
    public override void OnStopClient()
    {
        base.OnStopClient();
        print("OnStopClient");
    }
    // サーバーの停止時に呼ばれる
    public override void OnStopServer()
    {
        base.OnStopServer();
        print("OnStopServer");
    }
    //====================
    // クライアント
    //====================
    // クライアントの接続時に呼ばれる
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        print("OnClientConnect : " + conn.connectionId);
        // you can send the message here, or wherever else you want
        CreateMMOCharacterMessage characterMessage = new CreateMMOCharacterMessage
        {
            playerType = CharactorManager.playerType
        };

        NetworkClient.Send(characterMessage);
    }
    // クライアントの切断時に呼ばれる
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        print("OnClientDisconnect : " + conn.connectionId);
    }
    // クライアントのエラー時に呼ばれる
    public override void OnClientError(Exception exception)
    {
        base.OnClientError(exception);
        print("OnClientError : " + exception);
    }
    // クライアントの未準備時に呼ばれる
    public override void OnClientNotReady(NetworkConnection conn)
    {
        base.OnClientNotReady(conn);
        print("OnClientDisconnect : " + conn.connectionId);
    }
    // クライアントのシーン読み込み完了時に呼ばれる
    public override void OnClientChangeScene(string sceneName, SceneOperation sceneOperation, bool customHandlin)
    {
        base.OnClientChangeScene(sceneName, sceneOperation, customHandlin);
        print("OnClientChangeScene : " + sceneName);
    }
    //====================
    // サーバー
    //====================
    // サーバーの接続時に呼ばれる
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        print("OnServerConnect : " + conn.connectionId);
    }
    // サーバーの切断時に呼ばれる
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        print("OnServerDisconnect : " + conn.connectionId);
    }
    // サーバーの準備完了時に呼ばれる
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        print("OnServerReady : " + conn.connectionId);
    }
    // サーバーのエラー時に呼ばれる
    public override void OnServerError(NetworkConnection conn, Exception exception)
    {
        base.OnServerError(conn, exception);
        print("OnServerError : " + conn.connectionId + "," + exception);
    }
    // サーバーのプレイヤー追加時に呼ばれる
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        print("OnServerAddPlayer : " + conn.connectionId);
    }
    // サーバーのシーン読み込み完了時に呼ばれる
    public override void OnServerChangeScene(string sceneName)
    {
        base.OnServerChangeScene(sceneName);
        print("OnServerChangeScene : " + sceneName);
    }


    void OnCreateCharacter(NetworkConnection conn, CreateMMOCharacterMessage message)
    {
        //パワータイプキャラクターを生成
        if (PlayerType.piesu == message.playerType)
        {
            playerPrefab = piesu;
            Debug.Log("piesu");
        }
        //スピードタイプキャラクターを生成
        else if (PlayerType.pierun == message.playerType)
        {
            playerPrefab = pierun;
            Debug.Log("pierun");
        }
        //テクニックタイプキャラクターを生成
        else if (PlayerType.killsu == message.playerType)
        {
            playerPrefab = killsu;
            Debug.Log("killsu");
        }
        else if (PlayerType.killrun == message.playerType)
        {
            playerPrefab = killrun;
            Debug.Log("killrun");
        }
        //キャラクタータイプが選択されてない
        else
        {
            Debug.Log("Not chara selected");
            return;
        }

        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject gameobject = Instantiate(playerPrefab);

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);
        gamemode = NetworkServer.connections.Count;
    }

    public void OvO()
    {
        //接続人数が二人だった場合
        if (gamemode == 2)
        {
            ServerChangeScene("OvO");
        }
    }

    public static int GetPlayernum() 
    {
       return gamemode;
    }

}




