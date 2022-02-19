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
    GameObject SystemTime;              // タイムシステム
    bool isReady;                       // lobbyで準備完了フラグ

    public GameObject UI_Lobby;         // UI
    public GameObject UI_Game;          // UI
    public GameObject UI_Option;        // UI
    public GameObject UI_Result;        // UI

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
    public bool ATKflg;                 // アタックフラグ
    public bool Landflg;                // 着地フラグ
    public bool Stunflg;                // スタンフラグ
    public AudioSource source;
    int pastr;
    public Vector3 sonarPos;            // 
    float span = 2f;                    // 
    private float currentTime = 0f;     // 
    float ATKspan = 0.9f;               // 
    private float ATKcoolTime = 0f;     // 
    float Landspan = 1f;                // 
    private float LandcoolTime = 0f;    // 
    float SonarSpan = 0.8f;             // 走ってる
    private float SonarcoolTime = 0f;   // 走ってる
    float SoundSpan = 0.4f;             // 走ってる
    private float SoundcoolTime = 0f;   // 走ってる
    float StunSpan = 4f;                // スタン
    private float StunCoolTime = 0f;    // スタン
    bool OkATK;                         //
    float OkAtkSpan = 10f;              // 攻撃のクールタイム
    private float OkAtkCoolTime = 0f;   // 攻撃
    float SonarActSpan = 7f;            // 走ってる
    private float SonarActcoolTime = 0f;// 走ってる
    bool SonarAct;                      //



    void Start() {
        PlyRB = GetComponent<Rigidbody>();
        rb = this.transform.GetComponent<Rigidbody>();  // 自分のリジッドボディ
        SystemObj = GameObject.FindGameObjectWithTag("System");
        SystemTime = GameObject.FindGameObjectWithTag("Timer");
        Plane = GameObject.Find("y床");
        cam.GetComponent<AudioListener>().enabled = true;
        WLResult = false;
        ATKflg = false;
        Stunflg = false;
        OkATK = true;               // 攻撃可能
        SonarAct = true;

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
                    UI_Game.SetActive(false);
                    UI_Option.SetActive(false);
                    UI_Result.SetActive(false);
                    break;

                case GameMode.GAME:
                    Debug.Log("PLY Gamemode = Game");
                    isReady = false;
                    UI_Lobby.SetActive(false);
                    UI_Game.SetActive(true);
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        UI_Option.SetActive(true);
                    }

                    if (Killerflg == false)
                    {
                        if (SystemObj.GetComponent<GameSystemManage>().Escape == true)
                        {
                            currentTime += Time.deltaTime;
                            if (currentTime > span)
                            {
                                CmdSonarPlayerO(sonarPos, 10);
                                currentTime = 0f;
                            }
                        }
                    }
                    UI_Result.SetActive(false);
                    break;

                case GameMode.RESULT:
                    UI_Lobby.SetActive(false);
                    UI_Game.SetActive(false);
                    UI_Option.SetActive(false);
                    UI_Result.SetActive(true);
                    if( Killerflg == true)
                    {
                        UI_Result.transform.GetChild(3).gameObject.SetActive(true);
                        UI_Result.transform.GetChild(4).gameObject.SetActive(false);
                    }
                    else
                    {
                        UI_Result.transform.GetChild(3).gameObject.SetActive(false);
                        UI_Result.transform.GetChild(4).gameObject.SetActive(true);
                    }

                    if (WLResult == true)   // 勝った方
                    {
                        UI_Result.transform.GetChild(0).gameObject.SetActive(true);
                        UI_Result.transform.GetChild(1).gameObject.SetActive(false);
                    }
                    else　　　　　　　　　  // 負けたほう
                    {
                        UI_Result.transform.GetChild(0).gameObject.SetActive(false);
                        UI_Result.transform.GetChild(1).gameObject.SetActive(true);
                    }

                    if (isServer)
                    {
                        UI_Result.transform.GetChild(2).gameObject.SetActive(true);     // ロビーに戻るボタン
                    }
                    else
                    {
                        UI_Result.transform.GetChild(2).gameObject.SetActive(false);    // ロビーに戻るボタン
                    }
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
                    ATKflg = false;
                    if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)) ||
                        (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
                    {
                        plyMode = PlayerMode.WALK;
                        inputHorizontal = Input.GetAxisRaw("Horizontal");
                        inputVertical = Input.GetAxisRaw("Vertical");
                        if (plyMode == PlayerMode.WALK)
                        {
                            if (Input.GetKey(KeyCode.LeftShift))    // 走りキーを押しているか
                            {
                                plyMode = PlayerMode.RUN;
                            }
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

                    if (SystemObj.GetComponent<GameSystemManage>().gameMode == GameMode.GAME)
                    {
                        if (Killerflg == true)
                        {
                            if (Input.GetKey(KeyCode.F))
                            {
                                if (ATKflg == false)
                                {
                                    ATKflg = true;
                                    plyMode = PlayerMode.ATK;
                                }
                            }
                        }
                        else             // サバイバーの攻撃
                        {
                            if (OkATK == true)
                            {
                                if (Input.GetKey(KeyCode.F))
                                {
                                    if (ATKflg == false)
                                    {
                                        ATKflg = true;
                                        OkATK = false;
                                        plyMode = PlayerMode.ATK;
                                    }
                                }
                            }
                            else        // 攻撃クールダウン
                            {
                                OkAtkCoolTime += Time.deltaTime;
                                if (OkAtkCoolTime > OkAtkSpan)
                                {
                                    OkATK = true;
                                    OkAtkCoolTime = 0f;
                                }
                            }
                        }

                        if (plyMode == PlayerMode.RUN)
                        {
                            SoundcoolTime += Time.deltaTime;
                            if (SoundcoolTime > SoundSpan)
                            {
                                CmdPlaySounds(Player);  // 足音
                                SoundcoolTime = 0f;
                            }

                            SonarcoolTime += Time.deltaTime;
                            if (SonarcoolTime > SonarSpan)
                            {
                                if (Killerflg == true)
                                {
                                    CmdSonarPlayerR(sonarPos, 1.0f);
                                }
                                else
                                {
                                    CmdSonarPlayerB(sonarPos, 1.0f);
                                }
                                SonarcoolTime = 0f;
                            }
                        }
                    }

                    if (SonarAct == true)
                    {
                        if (Input.GetKey(KeyCode.Q))
                        {
                            if (Killerflg == true)
                            {
                                CmdSonarPlayerR(sonarPos, 1.0f);
                                SonarAct = false;
                            }
                            else
                            {
                                CmdSonarPlayerB(sonarPos, 1.0f);
                                SonarAct = false;
                            }
                        }
                    }
                    else
                    {
                        SonarActcoolTime += Time.deltaTime;
                        if (SonarActcoolTime > SonarActSpan)
                        {
                            SonarAct = true;
                            SonarActcoolTime = 0f;
                        }
                    }
                    break;

                case PlayerMode.ATK:    // 攻撃モード
                    ATKcoolTime += Time.deltaTime;
                    if (ATKcoolTime > ATKspan)
                    {
                        ATKflg = false;
                        plyMode = PlayerMode.WAIT;
                        ATKcoolTime = 0f;
                    }
                    CmdNotMovePlayer();
                    break;

                case PlayerMode.FALL:   // 落ちるモード
                    CmdNotMovePlayer();
                    break;

                case PlayerMode.LANDING:// 着地モード
                    LandcoolTime += Time.deltaTime;
                    if (LandcoolTime > Landspan)
                    {
                        Landflg = false;
                        plyMode = PlayerMode.WAIT;
                        LandcoolTime = 0f;
                    }
                    Debug.Log(Landflg);
                    CmdNotMovePlayer();
                    break;

                case PlayerMode.STUN:
                    StunCoolTime += Time.deltaTime;
                    if (StunCoolTime > StunSpan)
                    {
                        Stunflg = false;
                        plyMode = PlayerMode.WAIT;
                        StunCoolTime = 0f;
                    }
                    Debug.Log(Landflg);
                    break;
            }
        }
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        if (isLocalPlayer)
        {
            CmdSonarPlayerR(collision.contacts[0].point, collision.impulse.magnitude);
            CmdPlaySounds(Player);    // サーバーに送信
        }
    }*/


    // 当たり判定に当たっているとき
    void OnCollisionStay(Collision collision) {
        if (isLocalPlayer)
        {
            sonarPos = collision.contacts[0].point;
        }
    }


    // -------------------------------------------------------------------------------------------- //
    //                                                                                              //
    //                                  当たり判定関連                                              //
    //                                                                                              //
    // -------------------------------------------------------------------------------------------- //
    public void SurvivorCollisionHit()
    {
        if (isLocalPlayer)
        {
            Debug.Log("プレイヤー集合中");
            if (SystemObj.GetComponent<GameSystemManage>().Escape == false)
            {
                CmdEscapeSurvivor();
            }
        }
    }


    [Command]
    void CmdEscapeSurvivor()
    {
        SystemTime.GetComponent<TimerController>().estotalTime = Const.ESCAPE_TIME;
        SystemObj.GetComponent<GameSystemManage>().Escape = true;
    }
    public void SurvivorCollisionNotHit()
    {
        if (isLocalPlayer)
        {
            Debug.Log("プレイヤー集合していない");
            if (SystemObj.GetComponent<GameSystemManage>().Escape == true)
            {
                CmdNotEscapeSurvivor();
            }
        }
    }
    [Command]
    void CmdNotEscapeSurvivor()
    {
        Debug.Log("escapeをfalseに");
        SystemObj.GetComponent<GameSystemManage>().Escape = false;
    }

    public void MainCollisionHit(Collider other)
    {
        if (isLocalPlayer)
        {
            if (SystemObj.GetComponent<GameSystemManage>().gameMode == GameMode.GAME)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    Debug.Log("プレイヤーヒット");
                    KillerAttack(other);
                }
            }
        }
    }

    public void GunCollisionHit(Collider other)        // 逃げの銃のアクション
    {
        if (isLocalPlayer)
        {
            if (SystemObj.GetComponent<GameSystemManage>().gameMode == GameMode.GAME)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    Debug.Log("キラーヒット");
                    GunAttack(other);
                }
            }
        }
    }

    public void StandCollisionHit()     // 足元の当たり判定が当たったとき(着地)
    {
        if (plyMode == PlayerMode.FALL)
        {
            if (Landflg == false)
            {
                Landflg = true;
                plyMode = PlayerMode.LANDING;
            }
            else
            {
                Debug.Log("Land時のWAIT");
                plyMode = PlayerMode.WAIT;
            }
        }
    }

    public void LandCollisionHit()     // 足元の当たり判定が当たったとき(着地)
    {
        if ((plyMode == PlayerMode.FALL) || (plyMode == PlayerMode.LANDING))
        {
            plyMode = PlayerMode.WAIT;

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
    void CmdPlaySounds(GameObject main)
    {
        RpcPlaySounds(main);
    }
    [ClientRpc]
    void RpcPlaySounds(GameObject main)
    {
        main.GetComponent<AudioSource>().PlayOneShot(clips[0]);
    }

    // -----------------------------------------------------------------
    // プレイヤーのソナー
    // -----------------------------------------------------------------
    // 赤 ---------------------------
    [Command]
    void CmdSonarPlayerR(Vector4 point, float impulse)
    {
        RpcSonarPlayerR(point, impulse);
    }
    [ClientRpc]
    void RpcSonarPlayerR(Vector4 point, float impulse)
    {
        Debug.Log("赤");
        Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 3, new Color(1.0f, 0.0f, 0.0f, 1.0f));
    }
    // 青 ---------------------------
    [Command]
    void CmdSonarPlayerB(Vector4 point, float impulse)
    {
        RpcSonarPlayerB(point, impulse);
    }
    [ClientRpc]
    void RpcSonarPlayerB(Vector4 point, float impulse)
    {
        Debug.Log("青");
        Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 3, new Color(0.0f, 0.0f, 1.0f, 1.0f));
    }
    // オレンジ ---------------------------
    [Command]
    void CmdSonarPlayerO(Vector4 point, float impulse)
    {
        RpcSonarPlayerO(point, impulse);
    }
    [ClientRpc]
    void RpcSonarPlayerO(Vector4 point, float impulse)
    {
        Debug.Log("オレンジ");
        Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 3, new Color(1.0f, 0.5f, 0.0f, 1.0f));
    }



    // -----------------------------------------------------------------
    // 逃げの攻撃
    // -----------------------------------------------------------------
    void GunAttack(Collider other)
    {
        GameObject KillerObj = other.gameObject;
        if ( (Killerflg == false) && (KillerObj.gameObject.GetComponent<PlayerControl>().Stunflg == false) )
        {
            Debug.Log("キラーをスタンにするCMD");
            CmdAttackGun(KillerObj);
        }
    }

    [Command]
    void CmdAttackGun(GameObject KillerObj)
    {
        RpcAttackGun(KillerObj);
    }

    [ClientRpc]
    void RpcAttackGun(GameObject KillerObj)
    {
        Debug.Log("キラーをスタンにするCMD");
        KillerObj.gameObject.GetComponent<PlayerControl>().Stunflg = true;
        KillerObj.gameObject.GetComponent<PlayerControl>().plyMode = PlayerMode.STUN;
    }

    // -----------------------------------------------------------------
    // キラーの攻撃
    // -----------------------------------------------------------------
    void KillerAttack(Collider other)
    {
        if ((Killerflg == true) && (ATKflg == true))
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

    [ServerCallback]
    public void OnLobbyBack()
    {
        SystemObj.GetComponent<GameSystemManage>().gameMode = GameMode.LOBBY;
    }


}





