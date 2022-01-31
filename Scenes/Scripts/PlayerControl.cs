using UnityEngine;
using Mirror;

public class PlayerControl : NetworkBehaviour
{
    public Camera mycam;    // カメラコンポーネント
    public GameObject cam;  // カメラオブジェクト
    GameObject Plane;       // simple sonar用の床
    float inputHorizontal;  // 横のキー入力
    float inputVertical;    // 縦のキー入力
    Rigidbody rb;           // Rigid Body
    float moveSpeed;        // キャラの移動速度
    
    public int PlyObj = 0;  // 今どのモデルをONにしているか

    void Start() {
        Plane = GameObject.Find("y床");
        this.transform.GetChild(PlyObj).gameObject.SetActive(true);
        cam.GetComponent<AudioListener>().enabled = true;
    }

    void Update()
    {
        // キー入力
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }
    // 定期更新時に呼ばれる
    void FixedUpdate()
    {
        // 自分のrigidbodyを毎回取得
        rb = this.transform.GetComponent<Rigidbody>();
 //       rb = this.transform.GetChild(1).GetComponent<Rigidbody>();
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
            Debug.Log(netId);
            PlayerMovement();       // プレイヤーの移動を管理する関数
        }
    }

    void OnCollisionEnter()
    {
        if (isLocalPlayer)
        {
            this.transform.GetChild(1).GetComponent<AudioSource>().Play();
            if (isServer)
            {
                RpcPlaySounds();    // クライアントに送信
            }
            else if (isClient)
            {
                CmdPlaySounds();    // サーバーに送信
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude * 10);
                CmdPlaySounds();
            }
        }
    }


    [Command]
    void CmdPlaySounds()
    {
        this.transform.GetChild(PlyObj).GetComponent<AudioSource>().Play();
    }

    [ClientRpc]
    void RpcPlaySounds()
    {
        this.transform.GetChild(PlyObj).GetComponent<AudioSource>().Play();
    }


    // 当たり判定に当たったとき
    void OnTriggerStay(Collider other)
    {
        if (isLocalPlayer)
        {
            // 柵だった場合
            if (other.tag == "saku")
            {
                Debug.Log("柵にヒット");
                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log("乗り越えようとする");
                }
            }

            // プレイヤーだった場合
            if (other.tag == "Player")
            {
                Debug.Log("プレイヤーにヒット");
                if (Input.GetKey(KeyCode.C))
                {
                    Debug.Log("プレイヤーにタッチ");
                    //Systemobj.GetComponent<GameSystem>().CmdTouchPlayer(other.gameObject);
                    //CmdTouchPlayer(other.gameObject);
                }
            }
        }

    }


    // ---------------------------------------------- //
    //              プレイヤーの移動                  //
    // ---------------------------------------------- //
    void PlayerMovement()
    {
        // ダッシュ
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 5.0f;
        }
        else
        {
            moveSpeed = 3.0f;
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

    // プレイヤーの移動
    [Command]
    void CmdMovePlayer(Vector3 move)
    {
        GetComponent<Rigidbody>().velocity = move;
    }
    // プレイヤーの回転
    [Command]
    void CmdRotatePlayer(Quaternion rotate)
    {
        transform.rotation = rotate;
    }

}
