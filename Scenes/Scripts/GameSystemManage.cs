using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GrovalConst;

public class GameSystemManage : NetworkBehaviour
{
    public TimerController tManeger;
    public GameObject MainCameraDel;
    public GameObject spawnPlace;
    [SyncVar]
    public bool Escape;
    [SyncVar]
    public int escapeTime;
    [SyncVar]
    public int SyncSpawn;
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
        countPlayer = Const.MAX_PLAYER;
        SyncSpawn = 0;
        tManeger.totalTime = Const.START_TIME;
        tManeger.estotalTime = 10;
        Escape = false;
        escapeTime = 100;
        MainCameraDel.gameObject.SetActive(false);  // 接続した瞬間にカメラをメインカメラをオフにする
    }

    // Update is called once per frame
    void Update()
    {
        GameSystems();      // サーバーのゲームシステム
        tManeger.TimeSync();
        tManeger.esTimeSync();
    }


    [ServerCallback]
    void GameSystems()
    {
        switch (gameMode)
        {
            case GameMode.LOBBY:

                Debug.Log("Gamemode = Lobby");
                Debug.Log(SyncSpawn);
                if (readyPlayer == Const.MAX_PLAYER)
                {
                    tManeger.TimeInc();
                    tManeger.TimeSync();
                    if (tManeger.tim <= 0)
                    {
                        tManeger.totalTime = Const.GAME_TIME;
                        gameMode = GameMode.GAME;
                        GameObject[] ply = GameObject.FindGameObjectsWithTag("Player");
                        foreach (GameObject obj in ply)
                        {
                            SpawnPlayer(obj);
                        }
                        GameObject kill = GameObject.FindWithTag("Killer");
                        SpawnPlayer(kill);
                    }
                }
                break;

            case GameMode.GAME:
                tManeger.TimeInc();

                if (Escape == true)
                {
                    tManeger.esTimeInc();
                    GameObject plyPos = GameObject.FindWithTag("Player");
                    if (tManeger.estim <= 0)
                    {
                        GameObject[] ply = GameObject.FindGameObjectsWithTag("Player");
                        foreach (GameObject obj in ply)
                        {
                            Debug.Log("プレイヤー側の勝利");
                            obj.gameObject.GetComponent<PlayerControl>().WLResult = true;
                        }
                        GameObject kill = GameObject.FindWithTag("Killer");
                        kill.gameObject.GetComponent<PlayerControl>().WLResult = false;

                        gameMode = GameMode.RESULT;
                    }
                }


                if (tManeger.tim <= 0)      // タイムアップ時の処理
                {
                    GameObject[] ply = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject obj in ply)
                    {
                        obj.gameObject.GetComponent<PlayerControl>().WLResult = false;
                    }
                    GameObject kill = GameObject.FindWithTag("Killer");
                    kill.gameObject.GetComponent<PlayerControl>().WLResult = true;

                    gameMode = GameMode.RESULT;
                }
                break;

            case GameMode.RESULT:
                Debug.Log("Gamemode = Result");
                break;
        }

        if (SyncSpawn >= 7)
        {
            SyncSpawn = 0;
        }
    }

    // -----------------------------------------------------------------
    // スタート時にリス地(オブジェクトの場所)に飛ばす
    // -----------------------------------------------------------------
    [ServerCallback]
    void SpawnPlayer(GameObject Obj)
    {
        Debug.Log("ゲームシステム" + Obj.name);
        Obj.transform.position = spawnPlace.transform.GetChild(SyncSpawn).gameObject.transform.position;
        SyncSpawn++;
    }

}
