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
    public GameObject SelectKillerModel;
    public GameObject SelectKillerCamera;
    public GameObject SelectSurvivorModel;
    public GameObject SelectSurvivorCamera;
    [SyncVar]
    public bool Startflg;
    [SyncVar]
    public bool Escape;
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
        Startflg = false;
        SyncSpawn = 0;
        tManeger.estotalTime = 11;
        Escape = false;
        MainCameraDel.gameObject.SetActive(false);  // 接続した瞬間にカメラをメインカメラをオフにする
    }

    // Update is called once per frame
    void Update()
    {
        GameSystems();      // サーバーのゲームシステム
        tManeger.TimeSync();
        tManeger.esTimeSync();
        if (gameMode == GameMode.RESULT)
        {
            SelectKillerModel.SetActive(true);
            SelectSurvivorModel.SetActive(true);
            SelectKillerCamera.SetActive(true);
            SelectSurvivorCamera.SetActive(true);
        }
        else
        {
            SelectKillerModel.SetActive(false);
            SelectSurvivorModel.SetActive(false);
            SelectKillerCamera.SetActive(false);
            SelectSurvivorCamera.SetActive(false);
        }
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
                    if (Startflg == false)
                    {
                        Startflg = true;
                        tManeger.totalTime = Const.START_TIME;
                    }
                    else
                    {
                        tManeger.TimeInc();
                        tManeger.TimeSync();
                        if (tManeger.tim <= 0)
                        {
                            tManeger.totalTime = Const.GAME_TIME;
                            gameMode = GameMode.GAME;
                            readyPlayer = 0;
                            GameObject[] ply = GameObject.FindGameObjectsWithTag("Player");
                            foreach (GameObject obj in ply)
                            {
                                SpawnPlayer(obj);
                            }
                            GameObject kill = GameObject.FindWithTag("Killer");
                            SpawnPlayer(kill);
                        }
                    }
                }
                else
                {
                    Startflg = false;
                }
                break;

            case GameMode.GAME:
                tManeger.TimeInc();     // ゲーム中制限時間

                if (Escape == true)
                {
                    tManeger.esTimeInc();
                    GameObject plyPos = GameObject.FindWithTag("Player");
                    if (tManeger.estim <= 0)    // エスケープ成功
                    {
                        GameObject[] ply = GameObject.FindGameObjectsWithTag("Player");
                        foreach (GameObject obj in ply)
                        {
                            Debug.Log("プレイヤー側の勝利");
                            RpcWin(obj);
                        }
                        GameObject kill = GameObject.FindWithTag("Killer");
                        RpcLose(kill);

                        gameMode = GameMode.RESULT;
                    }
                }

                if (tManeger.tim <= 0)      // タイムアップ時の処理
                {
                    GameObject[] ply = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject obj in ply)
                    {
                        RpcLose(obj);
                    }
                    GameObject kill = GameObject.FindWithTag("Killer");
                    RpcWin(kill);

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

    // -----------------------------------------------------------------
    // 勝ち
    // -----------------------------------------------------------------
    [ClientRpc]
    void RpcWin(GameObject Obj)
    {
        Obj.gameObject.GetComponent<PlayerControl>().WLResult = true;
    }

    // -----------------------------------------------------------------
    // 負け
    // -----------------------------------------------------------------
    [ClientRpc]
    void RpcLose(GameObject Obj)
    {
        Obj.gameObject.GetComponent<PlayerControl>().WLResult = false;
    }




}
