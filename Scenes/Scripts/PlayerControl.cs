using UnityEngine;
using Mirror;
using GrovalConst;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PlayerControl : NetworkBehaviour
{
    [HideInInspector]
    public bool Killerflg;              // 自分が鬼であるかのフラグ
    [HideInInspector]
    public PlayerMode plyMode;          // プレイヤーのモード
    [HideInInspector]
    public bool WLResult;               // 勝ったか負けたか

    GameObject SystemObj;               // ゲームシステム
    bool isReady;                       // lobbyで準備完了フラグ

    public GameObject UI_Lobby;         // UI
    public GameObject UI_Game;          // UI
    public GameObject UI_Option;        // UI

    [SerializeField] AudioClip[] clips;
    [SerializeField] bool randomizePitch = true;
    [SerializeField] float pitchRange = 0.1f;
    public GameObject cam;              // 自分のカメラオブジェクト
    GameObject Plane;                   // 床オブジェクト
    public GameObject Player;           // 自分
    float inputHorizontal;              // 横の入力
    float inputVertical;                // 縦の入力
    Rigidbody rb;                       // リジッドボディ
    Rigidbody PlyRB;                    // プレイヤーのリジッドボディ
    float moveSpeed = Const.SPEED_WALK; // 移動の速さ
    bool trigger = false;               // トリガー
    public AudioSource source;
    int pastr;


    void Start() {
        PlyRB = GetComponent<Rigidbody>();
        rb = this.transform.GetComponent<Rigidbody>();  // 自分のリジッドボディ
        SystemObj = GameObject.FindGameObjectWithTag("System");
        Plane = GameObject.Find("y床");
        cam.GetComponent<AudioListener>().enabled = true;

        isReady = false;            // 準備未完了にする
        if (isClient) {             // サーバーを鬼にしてはじめる
            Killerflg = false;
        }
        if (isServer)
        {
            Killerflg = true;
        }

        plyMode = PlayerMode.WAIT;  // 立ちモードにする(スタンダード)
    }

    void Update()
    {
        // LocalPlayerで自分かどうか判断
        if (!isLocalPlayer)
        {
            cam.SetActive(false);       // カメラの有効化(自分の以外は無効に)
        }
        else
        {
            cam.SetActive(true);
            switch (SystemObj.GetComponent<GameSystemManage>().gameMode)
            {
                case GameMode.LOBBY:
                    Debug.Log("PLY Gamemode = Lobby");
                    UI_Lobby.SetActive(true);
                    if( isServer)
                    {
                        UI_Lobby.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    UI_Game.SetActive(true);
                    UI_Option.SetActive(false);
                    break;

                case GameMode.GAME:
                    Debug.Log("PLY Gamemode = Game");
                    UI_Lobby.SetActive(false);
                    UI_Game.SetActive(true);
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        UI_Option.SetActive(true);
                    }
                    break;

                case GameMode.RESULT:
                    Debug.Log(WLResult);
                    Debug.Log("PLY Gamemode = Result");
                    break;
            }
            ChangeKiller();        // 常に鬼かどうかを共有
        }
        Debug.Log(plyMode);         // 常にプレイヤーのモードを見る
    }

    // 定期更新時に呼ばれる
    void FixedUpdate()
    {
        // ローカルプレイヤーの時
        if (isLocalPlayer)
        {
            rb = this.transform.GetComponent<Rigidbody>();  // 自分のリジッドボディを毎回取り直す

            // プレイヤーのモードで動作を管理
            switch (plyMode)
            {
                case PlayerMode.WAIT:   // 立ちモード
                case PlayerMode.WALK:   // 歩きモード
                case PlayerMode.RUN:    // 走りモード
                    if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)) ||
                        (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
                    {
                        plyMode = PlayerMode.WALK;
                        inputHorizontal = Input.GetAxisRaw("Horizontal");
                        inputVertical = Input.GetAxisRaw("Vertical");
                        if (Input.GetKey(KeyCode.LeftShift))    // 走りキーを押しているか
                        {
                            plyMode = PlayerMode.RUN;
                        }
                        PlayerMovement();
                    }
                    else
                    {
                        plyMode = PlayerMode.WAIT;
                        inputHorizontal = 0;
                        inputVertical = 0;
                        CmdNotMovePlayer();
                    }
                    break;

                case PlayerMode.ATK:    // 攻撃モード
                    CmdNotMovePlayer();
                    break;

                case PlayerMode.FALL:   // 落ちるモード
                    CmdNotMovePlayer();
                    break;

                case PlayerMode.LANDING:// 着地モード
                    CmdNotMovePlayer();
                    break;
            }

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isLocalPlayer)
        {
            CmdSonarPlayer(collision.contacts[0].point, collision.impulse.magnitude);
            CmdPlaySounds();    // サーバーに送信
        }
    }


    // 当たり判定に当たっているとき
    void OnCollisionStay(Collision collision) {
        if (isLocalPlayer)
        {

            if (Input.GetKey(KeyCode.Q))
            {
                CmdSonarPlayer(collision.contacts[0].point, collision.impulse.magnitude);
                CmdPlaySounds();
            }

            if (Input.GetKey(KeyCode.Space))
            {
            }
        }
    }

    // -------------------------------------------------------------------------------------------- //
    //                                                                                              //
    //                                  当たり判定関連                                              //
    //                                                                                              //
    // -------------------------------------------------------------------------------------------- //
    public void SurvivorCollisionHit()
    {
        Debug.Log("プレイヤー集合中");
        if (SystemObj.GetComponent<GameSystemManage>().Escape == false)
        {
            CmdEscapeSurvivor();
        }
    }

    [Command]
    void CmdEscapeSurvivor()
    {
        SystemObj.GetComponent<GameSystemManage>().time = 10f;
        SystemObj.GetComponent<GameSystemManage>().Escape = true;
    }

    public void MainCollisionHit(Collider other)
    {
        if (isLocalPlayer)
        {
            Debug.Log("プレイヤーヒット");
            if (SystemObj.GetComponent<GameSystemManage>().gameMode == GameMode.GAME)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    KillerAttack(other);
                }
            }
        }
    }

    public void StandCollisionHit()     // 足元の当たり判定が当たったとき(着地)
    {
        if (plyMode == PlayerMode.FALL)
        {
            plyMode = PlayerMode.LANDING;
        }
    }

    public void StandCollisionExit()    // 足元の当たり判定が離れた時(落下)
    {
        plyMode = PlayerMode.FALL;
    }


    // -----------------------------------------------------------------
    // オプション画面を消す
    // -----------------------------------------------------------------
    public void OnclickOptionClose()
    {
        UI_Option.SetActive(false);
    }


    // -----------------------------------------------------------------
    // 鬼を変更する(呼べば勝手に変える)
    // -----------------------------------------------------------------
    void ChangeKiller()
    {
        if (Killerflg == true)
        {
            CmdChangeKiller();
        }
        else
        {
            CmdChangeSurvivor();
        }
    }


    // -----------------------------------------------------------------
    // プレイヤー鬼に交代
    // -----------------------------------------------------------------
    [Command]
    void CmdChangeKiller()
    {
        RpcChangeKiller();
    }

    [ClientRpc]
    void RpcChangeKiller()
    {
        Player.transform.GetChild(1).gameObject.SetActive(false);   // 1が逃げ
        Player.transform.GetChild(0).gameObject.SetActive(true);    // 0が鬼
        Player.tag = "Killer";
    }

    // -----------------------------------------------------------------
    // プレイヤー鬼から交代
    // -----------------------------------------------------------------
    [Command]
    void CmdChangeSurvivor()
    {
        RpcChangeSurvivor();
    }

    [ClientRpc]
    void RpcChangeSurvivor()
    {
        Player.transform.GetChild(0).gameObject.SetActive(false);   // 0が鬼
        Player.transform.GetChild(1).gameObject.SetActive(true);    // 1が逃げ
        Player.tag = "Player";
    }


    // -----------------------------------------------------------------
    // プレイヤーの回転
    // -----------------------------------------------------------------
    [Command]
    void CmdRotatePlayer(Quaternion rotate)
    {
        transform.rotation = rotate;
    }

    // -------------------------------------------------------------------------------------------- //
    //                                                                                              //
    //                                  プレイヤーの移動                                            //
    //                                                                                              //
    // -------------------------------------------------------------------------------------------- //
    void PlayerMovement()
    {
        if (plyMode == PlayerMode.RUN)    // 走っているか
        {
            moveSpeed = Const.SPEED_RUN;
        }
        else
        {
            moveSpeed = Const.SPEED_WALK;
        }

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + cam.transform.right * inputHorizontal;
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        var changed = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
        // 移動はサーバーにやらせる
        CmdMovePlayer(changed);
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            var rot = Quaternion.LookRotation(moveForward);
            CmdRotatePlayer(rot);       // 回転はサーバーにやらせる
        }

    }


    [Command]
    void CmdMovePlayer(Vector3 move)
    {
        PlyRB.velocity = move;
    }

    [Command]
    void CmdNotMovePlayer()
    {
        PlyRB.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
        PlyRB.angularVelocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
    }

    // -----------------------------------------------------------------
    // プレイヤーの音
    // -----------------------------------------------------------------
    [Command]
    void CmdPlaySounds()
    {
        RpcPlaySounds();
    }

    [ClientRpc]
    void RpcPlaySounds()
    {
        source.PlayOneShot(clips[0]);
    }


    // -----------------------------------------------------------------
    // プレイヤーのソナー
    // -----------------------------------------------------------------
    [Command]
    void CmdSonarPlayer(Vector4 Point, float impulse) {
        RpcSonarPlayer(Point, impulse);
    }

    [ClientRpc]
    void RpcSonarPlayer(Vector4 point, float impulse)
    {
        Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10);
    }



    // -----------------------------------------------------------------
    // キラーの攻撃
    // -----------------------------------------------------------------
    void KillerAttack(Collider other)
    {
        if (Killerflg == true)
        {
            GameObject receive = other.gameObject;
            int range = Random.Range(0, 8);
            Vector3 AttackPlace = new Vector3(0, 0, 0);
            Vector3 ReceivePlace = new Vector3(0, 0, 0);
            int r;
            Debug.Log("当たった");
            switch (range)
            {
                case 0:
                    AttackPlace = new Vector3(0, 5, -60);
                    break;
                case 1:
                    AttackPlace = new Vector3(25, 5, -25);
                    break;
                case 2:
                    AttackPlace = new Vector3(20, 5, 15);
                    break;
                case 3:
                    AttackPlace = new Vector3(-40, 5, 40);
                    break;
                case 4:
                    AttackPlace = new Vector3(-40, 5, -20);
                    break;
                case 5:
                    AttackPlace = new Vector3(30, 25, -5);
                    break;
                case 6:
                    AttackPlace = new Vector3(-35, 25, 35);
                    break;
                case 7:
                    AttackPlace = new Vector3(10, 25, -20);
                    break;
            }
            r = range;
            while (true)
            {
                r = Random.Range(0, 8);
                if (r != range && r != pastr)
                {
                    break;
                }
            }
            pastr = r;
            switch (r)
            {
                case 0:
                    ReceivePlace = new Vector3(0, 5, -60);
                    break;
                case 1:
                    ReceivePlace = new Vector3(25, 5, -25);
                    break;
                case 2:
                    ReceivePlace = new Vector3(20, 5, 15);
                    break;
                case 3:
                    ReceivePlace = new Vector3(-40, 5, 40);
                    break;
                case 4:
                    ReceivePlace = new Vector3(-40, 5, -20);
                    break;
                case 5:
                    ReceivePlace = new Vector3(30, 25, -5);
                    break;
                case 6:
                    ReceivePlace = new Vector3(-35, 25, 35);
                    break;
                case 7:
                    ReceivePlace = new Vector3(10, 25, -20);
                    break;
            }
            CmdAttackPlayer(Player, receive, AttackPlace, ReceivePlace);
            Killerflg = false;
            Debug.Log("逃げ飛ばす");
        }
    }


    [Command]
    void CmdAttackPlayer(GameObject main, GameObject opponent, Vector3 attacker, Vector3 receiver)
    {
        RpcAttackPlayer(main, opponent, attacker, receiver);
    }
    [ClientRpc]
    void RpcAttackPlayer(GameObject main, GameObject opponent, Vector3 attacker, Vector3 receiver)
    {
        main.transform.position = attacker;
        opponent.transform.position = receiver;
        opponent.gameObject.GetComponent<PlayerControl>().Killerflg = true;
    }

    // -----------------------------------------------------------------
    // プレイヤーの準備完了
    // -----------------------------------------------------------------
    public void OnClickReady()
    {
        if (isReady == false)
        {
            isReady = true;
            if (isClient)
            {
                CmdIncSync();
            }
        }
        else
        {
            isReady = false;
            if (isClient)
            {
                CmdDecreSync();
            }
        }
    }

    [ClientRpc]
    public void RpcReadyInc()
    {
        SystemObj.GetComponent<GameSystemManage>().readyPlayer++;
    }

    [Command]
    public void CmdIncSync()
    {
        RpcReadyInc();
    }

    [ClientRpc]
    public void RpcReadyDecre()
    {
        SystemObj.GetComponent<GameSystemManage>().readyPlayer--;
    }

    [Command]
    public void CmdDecreSync()
    {
        RpcReadyDecre();
    }

 


}





