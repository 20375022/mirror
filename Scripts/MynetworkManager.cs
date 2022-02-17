using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;


public class MynetworkManager : NetworkManager
{
    //キャラクター
    public GameObject killgaki;         // ジェーソンとガキ
    public GameObject killrun;          // ジェーソンとランナー
    public GameObject killOL;           // ジェーソンとOL
    public GameObject piegaki;          // ピエロとガキ
    public GameObject pierun;           // ピエロとランナー
    public GameObject pieOL;            // ピエロとOL
    public GameObject pastgaki;         // ペストとガキ
    public GameObject pastrun;          // ペストとランナー
    public GameObject pastOL;           // ペストとOL

    public GameObject Spawner;          // 最初のスポーン位置
    public GameObject place;            // リスポーン地点
    public int CreateSpawn;

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
        SceneManager.LoadScene("title");
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
        if (PlayerType.piegaki == message.playerType)
        {
            playerPrefab = piegaki;
            Debug.Log("piegaki");
        }
        else if (PlayerType.pierun == message.playerType)
        {
            playerPrefab = pierun;
            Debug.Log("pierun");
        }
        else if (PlayerType.pieOL == message.playerType)
        {
            playerPrefab = pieOL;
            Debug.Log("pierun");
        }
        else if (PlayerType.killgaki == message.playerType)
        {
            playerPrefab = killgaki;
            Debug.Log("killsu");
        }
        else if (PlayerType.killrun == message.playerType)
        {
            playerPrefab = killrun;
            Debug.Log("killrun");
        }
        else if (PlayerType.killOL == message.playerType)
        {
            playerPrefab = killOL;
            Debug.Log("killsu");
        }
        else if (PlayerType.pastgaki == message.playerType)
        {
            playerPrefab = pastgaki;
            Debug.Log("killsu");
        }
        else if (PlayerType.pastrun == message.playerType)
        {
            playerPrefab = pastrun;
            Debug.Log("killrun");
        }
        else if (PlayerType.pastOL == message.playerType)
        {
            playerPrefab = pastOL;
            Debug.Log("killsu");
        }
        //キャラクタータイプが選択されてない
        else
        {
            Debug.Log("Not chara selected");
            return;
        }

        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        Transform startPos = GetNetStartPosition();        // スタートポジションがアタッチされたオブジェクトの座標を探す
        GameObject gameobject= startPos != null             // オブジェクトがあればその位置にInstantiateする
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        Destroy(Spawner.transform.GetChild(CreateSpawn).gameObject);
        //GameObject gameobject = Instantiate(playerPrefab);
        //GameObject gameobject = Instantiate(playerPrefab, place.transform.GetChild(CreateSpawn).gameObject.transform.position, Quaternion.identity);
        //CreateSpawn++;

        // call this to use this gameobject as the primary controller
        gameobject.name = $"{gameobject.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

    /// <summary>Get the next NetworkStartPosition based on the selected PlayerSpawnMethod.</summary>
    public Transform GetNetStartPosition()
    {
        // first remove any dead transforms
        startPositions.RemoveAll(t => t == null);

        if (startPositions.Count == 0)
            return null;

        if (playerSpawnMethod == PlayerSpawnMethod.Random)
        {
            CreateSpawn = UnityEngine.Random.Range(0, startPositions.Count);
            return startPositions[CreateSpawn];
        }
        else
        {
            Transform startPosition = startPositions[startPositionIndex];
            startPositionIndex = (startPositionIndex + 1) % startPositions.Count;
            return startPosition;
        }
    }

}




