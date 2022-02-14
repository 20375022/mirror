using UnityEngine;
using Mirror;

public class PlayerControl : NetworkBehaviour
{
    public Camera mycam;
    public GameObject cam;
    public GameObject Plane;
    public GameObject Player;
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;
    float moveSpeed = 3f;
    bool trigger = false;
    bool sonarkey = false;

    void Start() {
        Plane = GameObject.Find("地面");
        cam.GetComponent<AudioListener>().enabled = true;
        if (isLocalPlayer){
            if (isClient){
                Player.tag = "Player";
            }
            if (isServer){
                Player.tag = "Killer";
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            sonarkey = true;
        }
        else {
            sonarkey = false;
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
                RpcPlaySounds();    // クライアントに送信
            }
            else if (isClient)
            {
                CmdPlaySounds();    // サーバーに送信
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        if (isLocalPlayer)
        {
            if (isServer)
            {
                RqcSonarPlayer(collision.contacts[0].point, collision.impulse.magnitude, sonarkey);
                RpcPlaySounds();
            }
            else if (isClient){
                if (sonarkey == true){
                    CmdClientSonar(collision.contacts[0].point, collision.impulse.magnitude);
                }
                CmdPlaySounds();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (isLocalPlayer){
            if (Input.GetKey(KeyCode.Space)){
                if (other.CompareTag("Player"))
                {
                    GameObject receive = GameObject.Find(other.gameObject.name);
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
                    switch (range){
                        case 0 :
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

                    Debug.Log("x_z" + ax + az);

                    Vector3 AttackPlace = new Vector3(ax, ay, az);

                    r = range;
                    while (r == range) {
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
                }
            }
        }
    }

    [Command]
    void CmdPlaySounds()
    {
        RpcPlaySounds();
    }

    [ClientRpc]
    void RpcPlaySounds()
    {
        this.GetComponent<AudioSource>().Play();
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
    void CmdSonarPlayer(Vector4 point, float impulse, bool s_sonarkey) {
        if (s_sonarkey == true)
        {
            Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10);
        }
    }
    [ClientRpc]
    void RqcSonarPlayer(Vector4 point, float impulse, bool c_sonarkey)
    {
        if (c_sonarkey == true)
        {
            Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10);
        }
    }
}