using UnityEngine;
using Mirror;

public class PlayerControl : NetworkBehaviour
{
    public Camera mycam;
    public GameObject cam;
    public GameObject Plane;
    public GameObject Player;
    PlayerControl change;
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;
    float moveSpeed = 3f;
    bool trigger = false;
    bool ZKey = false;

    void Start() {
        Plane = GameObject.Find("地面");
        if (isServer){
            Player.name = "S_Player";
        }
        if (isClient){
            change = GameObject.Find("S_Player").GetComponent<PlayerControl>();
        }
        cam.GetComponent<AudioListener>().enabled = true;
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


    void OnCollisionStay(Collision collision) {
        if (isLocalPlayer)
        {
            if (isServer) {
                RqcSonarPlayer(collision.contacts[0].point, collision.impulse.magnitude, ZKey);
            }
            else if (isClient){
                if (ZKey == true){
                    CmdClientSonar(collision.contacts[0].point, collision.impulse.magnitude);
                }
            }
            //CmdPlaySounds();
        }
    }

    // プレイヤーの回転
    [Command]
    void CmdRotatePlayer(Quaternion rotate)
    {
        transform.rotation = rotate;
    }
    // プレイヤーの移動
    [Command]
    void CmdMovePlayer(Vector3 move)
    {
        GetComponent<Rigidbody>().velocity = move;
    }

    [Command]
    void CmdClientSonar(Vector4 Point, float impulse) {
        RqcSonarPlayer(Point, impulse, true);
    }

    [Command]
    void CmdSonarPlayer(Vector4 point, float impulse, bool s_ZKey) {
        if (s_ZKey == true)
        {
            Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10);
        }
    }
    [ClientRpc]
    void RqcSonarPlayer(Vector4 point, float impulse, bool c_ZKey)
    {
        if (c_ZKey == true)
        {
            Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10);
        }
    }
}