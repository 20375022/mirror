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

    GameObject SystemObj;               // ゲームシステム
    bool isReady;                       // lobbyで準備完了フラグ

    public GameObject UI_Lobby;         // UI
    public GameObject UI_Game;          // UI
    public GameObject UI_Option;        // UI

    [SerializeField] AudioClip[] clips;
    [SerializeField] bool randomizePitch = true;
    [SerializeField] float pitchRange = 0.1f;
    public Camera mycam;                // カメラコンポーネント
    public GameObject cam;              // 自分のカメラオブジェクト
    GameObject Plane;                   // 床オブジェクト
    public GameObject Player;           // 自分
    float inputHorizontal;              // 横の入力
    float inputVertical;                // 縦の入力
    Rigidbody rb;                       // リジッドボディ
    float moveSpeed = Const.SPEED_WALK; // 移動の速さ
    bool trigger = false;               // トリガー
    public AudioSource source;

    void Start() {
        isReady = false;
        SystemObj = GameObject.FindGameObjectWithTag("System");
        Plane = GameObject.Find("y床");
        cam.GetComponent<AudioListener>().enabled = true;
        if (isClient) {
            Debug.Log("client");
            Killerflg = false;
            Debug.Log(Killerflg);
        }
        if (isServer)
        {
            Debug.Log("server");
            Killerflg = true;
            Debug.Log(Killerflg);
        }
        ChangeKiller(Killerflg);    // 最初に鬼かどうか判断
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        switch (SystemObj.GetComponent<GameSystemManage>().gameMode)
        {
            case GameMode.LOBBY:

                Debug.Log("PLY Gamemode = Lobby");
                UI_Lobby.SetActive(true);
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
                Debug.Log("PLY Gamemode = Result");
                break;
        }

    }

    // 定期更新時に呼ばれる
    void FixedUpdate()
    {
        rb = this.transform.GetComponent<Rigidbody>();
        // カメラの有効化(自分の以外は無効に)
        if (!isLocalPlayer)
        {
            cam.SetActive(false);
        }
        else
        {
            cam.SetActive(true);
        }

        // ローカルプレイヤーの時
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.LeftShift))    // 走っているか
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
                if (Killerflg == true)
                {
                    Killerflg = false;
                }
                else
                {
                    Killerflg = true;
                }
                Debug.Log(Killerflg);
                ChangeKiller(Killerflg);

            }
            //CmdPlaySounds();
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.F))
            {
                if (other.CompareTag("Player"))
                {
                    if (Killerflg == true)
                    {
                        GameObject receive = other.gameObject;
                        int range = Random.Range(0, 8);
                        int ax, ay, az;
                        int rx, ry, rz;
                        int r;
                        ax = 0;
                        ay = 0;
                        az = 0;
                        rx = 0;
                        ry = 0;
                        rz = 0;
                        Debug.Log("当たった");
                        switch (range)
                        {
                            case 0:
                                ax = -90;
                                az = -90;
                                ay = 1;
                                break;
                            case 1:
                                ax = -90;
                                az = 0;
                                ay = 1;
                                break;
                            case 2:
                                ax = 0;
                                az = -90;
                                ay = 1;
                                break;
                            case 3:
                                ax = -90;
                                az = 90;
                                ay = 1;
                                break;
                            case 4:
                                ax = 90;
                                az = -90;
                                ay = 3;
                                break;
                            case 5:
                                ax = 0;
                                az = 90;
                                ay = 1;
                                break;
                            case 6:
                                ax = 90;
                                az = 0;
                                ay = 3;
                                break;
                            case 7:
                                ax = 90;
                                az = 90;
                                ay = 6;
                                break;
                        }
                        Vector3 AttackPlace = new Vector3(ax, ay, az);
                        r = range;
                        while (r == range)
                        {
                            r = Random.Range(0, 8);
                        }
                        switch (r)
                        {
                            case 0:
                                rx = -90;
                                rz = -90;
                                ry = 1;
                                break;
                            case 1:
                                rx = -90;
                                rz = 0;
                                ry = 1;
                                break;
                            case 2:
                                rx = 0;
                                rz = -90;
                                ry = 1;
                                break;
                            case 3:
                                rx = -90;
                                rz = 90;
                                ry = 1;
                                break;
                            case 4:
                                rx = 90;
                                rz = -90;
                                ry = 3;
                                break;
                            case 5:
                                rx = 0;
                                rz = 90;
                                ry = 1;
                                break;
                            case 6:
                                rx = 90;
                                rz = 0;
                                ry = 3;
                                break;
                            case 7:
                                rx = 90;
                                rz = 90;
                                ry = 6;
                                break;
                        }
                        Vector3 ReceivePlace = new Vector3(rx, ry, rz);
                        this.transform.position = AttackPlace;
                        receive.transform.position = ReceivePlace;
                        //receive.transform.position = new Vector3(90, 0, -80);
                        Debug.Log("逃げ飛ばす");
                    }
                }
            }
        }
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
    void ChangeKiller(bool Killerflg)
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
    }


    // -----------------------------------------------------------------
    // プレイヤーの回転
    // -----------------------------------------------------------------
    [Command]
    void CmdRotatePlayer(Quaternion rotate)
    {
        transform.rotation = rotate;
    }
    // -----------------------------------------------------------------
    // プレイヤーの移動
    // -----------------------------------------------------------------
    [Command]
    void CmdMovePlayer(Vector3 move)
    {
        GetComponent<Rigidbody>().velocity = move;
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