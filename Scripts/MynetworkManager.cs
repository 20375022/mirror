﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*public struct CreateMMOCharacterMessage : NetworkMessage
{
    public PlayerType playerType;
}*/

public class MynetworkManager : NetworkManager
{
    //パワーキャラクター
    public GameObject power;
    //スピードキャラクター
    public GameObject speed;
    //テクニックキャラクター
    public GameObject technique;

    static int gamemode = 0;

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreateMMOCharacterMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        // you can send the message here, or wherever else you want
        CreateMMOCharacterMessage characterMessage = new CreateMMOCharacterMessage
        {
            playerType = CharactorManager.playerType
        };

        conn.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnection conn, CreateMMOCharacterMessage message)
    {
        //パワータイプキャラクターを生成
        if (PlayerType.power == message.playerType)
        {
            playerPrefab = power;
            Debug.Log("power");
        }
        //スピードタイプキャラクターを生成
        else if (PlayerType.speed == message.playerType)
        {
            playerPrefab = speed;
            Debug.Log("speed");
        }
        //テクニックタイプキャラクターを生成
        else if (PlayerType.tec == message.playerType)
        {
            playerPrefab = technique;
            Debug.Log("technique");
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




