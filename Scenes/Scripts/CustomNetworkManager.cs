using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public Canvas canvas;

    //====================
    // ホスト・クライアント・サーバーの開始・停止
    //====================
    // ホストの開始時に呼ばれる
    public override void OnStartHost()
    {
        base.OnStartHost();
        print("OnStartHost");
        canvas.gameObject.SetActive(false);
    }

    // クライアントの開始時に呼ばれる
    public override void OnStartClient()
    {
        base.OnStartClient();
        print("OnStartClient");
        canvas.gameObject.SetActive(false);
    }

    // サーバーの開始時に呼ばれる
    public override void OnStartServer()
    {
        base.OnStartServer();
        print("OnStartServer");
        canvas.gameObject.SetActive(false);
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
    public void OnClientNotReady(NetworkConnection conn)
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
    public void OnServerAddPlayer(NetworkConnection conn)
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
}