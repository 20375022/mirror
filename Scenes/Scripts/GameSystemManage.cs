using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GrovalConst;

public class GameSystemManage : NetworkBehaviour
{
    public TimerController tManeger;
    public GameObject MainCameraDel;
    [SyncVar]
    public GameMode gameMode;
    [SyncVar]
    public int countPlayer;
    [SyncVar]
    public int readyPlayer;

    // Start is called before the first frame update
    void Start()
    {
        gameMode = GameMode.LOBBY;
        readyPlayer = 0;
        countPlayer = 4;
        tManeger.totalTime = Const.START_TIME;
        MainCameraDel.gameObject.SetActive(false);  // 接続した瞬間にカメラをメインカメラをオフにする
    }

    // Update is called once per frame
    void Update()
    {
        GameSystems();
        tManeger.TimeSync();
    }

    [ServerCallback]
    void GameSystems()
    {
        switch (gameMode)
        {
            case GameMode.LOBBY:

                Debug.Log("Gamemode = Lobby");
                if (readyPlayer == Const.MAX_PLAYER)
                {
                    tManeger.TimeInc();
                    tManeger.TimeSync();
                    if (tManeger.tim <= 0)
                    {
                        tManeger.totalTime = Const.GAME_TIME;
                        gameMode = GameMode.GAME;
                    }
                }
                break;

            case GameMode.GAME:
                Debug.Log("Gamemode = Game");
                tManeger.TimeInc();
                if (tManeger.tim <= 0)
                {
                    gameMode = GameMode.RESULT;
                }
                break;

            case GameMode.RESULT:
                Debug.Log("Gamemode = Result");
                break;
        }
    }

}
