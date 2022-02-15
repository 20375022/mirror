using UnityEngine;
using Mirror;
using GrovalConst;

public class PlayerControl : NetworkBehaviour
{
    [HideInInspector]
    public bool Killerflg;              // 自分が鬼であるかのフラグ

    GameObject SystemObj;               // ゲームシステム
    bool isReady;                       // lobbyで準備完了フラグ

    public GameObject UI_Lobby;         // UI
    public GameObject UI_Game;          // UI
    public GameObject UI_Option;        // UI

    public Camera mycam;                // カメラコンポーネント
    public GameObject cam;              // 自分のカメラオブジェクト
    GameObject Plane;                   // 床オブジェクト
    public GameObject Player;           // 自分
    float inputHorizontal;              // 横の入力
    float inputVertical;                // 縦の入力
    Rigidbody rb;                       // リジッドボディ
    float moveSpeed = Const.SPEED_WALK; // 移動の速さ
    bool trigger = false;               // トリガー
    bool ZKey = false;                  // Zキー

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
        if (Input.GetKey(KeyCode.Z))
        {
            ZKey = true;
        }
        else {
            ZKey = false;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        switch (SystemObj.GetComponent<GameSystemManage>().gameMode)
        {
            case (int)GameMode.LOBBY:

                Debug.Log("PLY Gamemode = Lobby");
                UI_Lobby.SetActive(true);
                UI_Game.SetActive(false);
                UI_Option.SetActive(false);
                break;

            case (int)GameMode.GAME:
                Debug.Log("PLY Gamemode = Game");
                UI_Lobby.SetActive(false);
                UI_Game.SetActive(true);
                break;

            case (int)GameMode.RESULT:
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

    void OnCollisionEnter()
    {
        if (isLocalPlayer)
        {
            this.GetComponent<AudioSource>().Play();
            if (isServer)
            {
                //RpcPlaySounds();    // クライアントに送信
            }
            else if (isClient)
            {
                //CmdPlaySounds();    // サーバーに送信
            }
        }
    }


    [Command]
    void CmdPlaySounds()
    {
        this.GetComponent<AudioSource>().Play();
    }

    [ClientRpc]
    void RpcPlaySounds()
    {
        this.GetComponent<AudioSource>().Play();
    }

    // 当たり判定に当たっているとき
    void OnCollisionStay(Collision collision) {
        if (isLocalPlayer)
        {
            if (isServer) {
                RqcSonarPlayer(collision.contacts[0].point, collision.impulse.magnitude, ZKey);
            }
            else if (isClient) {
                if (ZKey == true) {
                    CmdClientSonar(collision.contacts[0].point, collision.impulse.magnitude);
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
/*                if (Killerflg == true)
                {
                    Killerflg = false;
                }
                else
                {
                    Killerflg = true;
                }
                Debug.Log(Killerflg);
                ChangeKiller(Killerflg);
*/
            }
            //CmdPlaySounds();
        }
    }



    // -----------------------------------------------------------------
    // 鬼を変更する(呼べば勝手に変える)
    // -----------------------------------------------------------------
    void ChangeKiller(bool Killerflg)
    {
        if (isServer)
        {
            if (Killerflg == true)
            {
                RpcChangeKiller();
            }
            else
            {
                RpcChangeSurvivor();
            }
        }
        else if (isClient)
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
    // プレイヤーのソナー
    // -----------------------------------------------------------------
    [Command]
    void CmdClientSonar(Vector4 Point, float impulse) {
        RqcSonarPlayer(Point, impulse, true);
    }

    [ClientRpc]
    void RqcSonarPlayer(Vector4 point, float impulse, bool c_ZKey)
    {
        if (c_ZKey == true)
        {
            Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10);
        }
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