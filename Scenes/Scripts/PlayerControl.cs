using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PlayerControl : NetworkBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] bool randomizePitch = true;
    [SerializeField] float pitchRange = 0.1f;
    public Material[] matsonar;
    public Camera mycam;
    public GameObject cam;
    public GameObject Plane;
    public GameObject Player;
    public AudioSource source;
    public Collider killattack;
    public Collider stungun;
    private int pastr = 0;
    private bool iskiller;
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;
    float moveSpeed = 3f;
    bool trigger = false;


    void Start() {
        Plane = GameObject.Find("地面");
        cam.GetComponent<AudioListener>().enabled = true;
        if (isLocalPlayer){
            if (isClient){
                CmdKillerOrSurvivor(this, false);
//                iskiller = false;
//                CmdKillerOrSurvivor("Player") ;
//                killattack.enabled = false;
            }
            if (isServer){
                CmdKillerOrSurvivor(this, true);
//                iskiller = true;
//                CmdKillerOrSurvivor("Killer");
//                stungun.enabled = false;
            }
        }
    }

    void Update()
    {
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

    /*+++++++++++++++++++++++++++++++++++++++++++++
     |      接地(瞬間)
     +++++++++++++++++++++++++++++++++++++++++++++*/
    void OnCollisionEnter(Collision collision)
    {
        if (isLocalPlayer)
        {
            Debug.Log("座標" + collision.contacts[0].point);
//            CmdSonarPlayerR(collision.contacts[0].point, collision.impulse.magnitude);
            CmdPlaySounds(Player);
        }
    }

    /*+++++++++++++++++++++++++++++++++++++++++++++
     |      接地(継続)
     +++++++++++++++++++++++++++++++++++++++++++++*/
    void OnCollisionStay(Collision collision)
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
            {
                if (trigger == false)
                {
                    trigger = true;
/*                    for (int i = 0; i < matsonar.GetLength(0); i++)
                    {
                        matsonar[i].SetColor("_RingColor", new Color(1.0f, 0.0f, 0.0f, 1.0f));
                    }*/
                    if (Input.GetKey(KeyCode.Q))
                    {
//                        Plane.GetComponent<SimpleSonarShader_Object>().SonarColor(1.0f, 0.0f, 0.0f);
                        CmdSonarPlayerR(collision.contacts[0].point, collision.impulse.magnitude);
                    }
                    else {
//                        Plane.GetComponent<SimpleSonarShader_Object>().SonarColor(0.0f, 0.0f, 1.0f);
                        CmdSonarPlayerB(collision.contacts[0].point, collision.impulse.magnitude);
                    }
//                    CmdSonarPlayerR(collision.contacts[0].point, collision.impulse.magnitude, new Color(1.0f, 0.0f, 0.0f));
                    CmdPlaySounds(Player);
                }
            }
            else {
                trigger = false;
            }
/*                if (Input.GetKey(KeyCode.E))
            {
                for (int i = 0; i < matsonar.GetLength(0); i++)
                {
                    matsonar[i].SetColor("_RingColor", new Color(0.0f, 0.0f, 1.0f, 1.0f));
                }
                CmdSonarPlayerB(collision.contacts[0].point, collision.impulse.magnitude);
                CmdPlaySounds(Player);
            }*/
        }
    }

    /*+++++++++++++++++++++++++++++++++++++++++++++
     |      当たり判定(継続)
     +++++++++++++++++++++++++++++++++++++++++++++*/
    void OnTriggerStay(Collider other)
    {
        if (isLocalPlayer){
            if (Input.GetKey(KeyCode.Space)){
                if (other.CompareTag("Player"))
                {
                    GameObject receive = other.gameObject;
                    int range = Random.Range(0, 8);
                    Vector3 AttackPlace = new Vector3(0, 0, 0);
                    Vector3 ReceivePlace = new Vector3(0, 0, 0);
                    int r;

                    Debug.Log("当たった");
                    switch (range){
                        case 0 :
                            AttackPlace = new Vector3(-90, 1, -90);
                            break;
                        case 1:
                            AttackPlace = new Vector3(-90, 1, 0);
                            break;
                        case 2:
                            AttackPlace = new Vector3(0, 1, -90);
                            break;
                        case 3:
                            AttackPlace = new Vector3(-90, 1, 90);
                            break;
                        case 4:
                            AttackPlace = new Vector3(90, 3, -90);
                            break;
                        case 5:
                            AttackPlace = new Vector3(0, 1, 90);
                            break;
                        case 6:
                            AttackPlace = new Vector3(90, 3, 0);
                            break;
                        case 7:
                            AttackPlace = new Vector3(90, 6, 90);
                            break;
                    }

                    r = range;
                    while (true) {
                        r = Random.Range(0, 8);
                        if (r != range && r != pastr){
                            break ;
                        }
                    }
                    pastr = r;

                    switch (r)
                    {
                        case 0:
                            ReceivePlace = new Vector3(-90, 1, -90);
                            break;
                        case 1:
                            ReceivePlace = new Vector3(-90, 1, 0);
                            break;
                        case 2:
                            ReceivePlace = new Vector3(0, 1, -90);
                            break;
                        case 3:
                            ReceivePlace = new Vector3(-90, 1, 90);
                            break;
                        case 4:
                            ReceivePlace = new Vector3(90, 3, -90);
                            break;
                        case 5:
                            ReceivePlace = new Vector3(0, 1, 90);
                            break;
                        case 6:
                            ReceivePlace = new Vector3(90, 3, 0);
                            break;
                        case 7:
                            ReceivePlace = new Vector3(90, 6, 90);
                            break;
                    }
                    Debug.Log("x_z" + ReceivePlace.x + " - " + ReceivePlace.z);

                    CmdResetPlayer(Player, receive, AttackPlace, ReceivePlace);
                }
            }
        }
    }

    /*+++++++++++++++++++++++++++++++++++++++++++++
     |      鬼逃げ分岐
     +++++++++++++++++++++++++++++++++++++++++++++*/
    [Command]
    void CmdKillerOrSurvivor(PlayerControl pc, bool kos) {
        RpcKillerOrSurvivor(pc, kos) ;
    }

    [ClientRpc]
    void RpcKillerOrSurvivor(PlayerControl pc, bool kos)
    {
        pc.iskiller = kos;
        Debug.Log("分岐時点" + pc.iskiller);
    }


    /*+++++++++++++++++++++++++++++++++++++++++++++
     |      音発生
     +++++++++++++++++++++++++++++++++++++++++++++*/
    [Command]
    void CmdPlaySounds(GameObject main)
    {
        RpcPlaySounds(main);
    }

    [ClientRpc]
    void RpcPlaySounds(GameObject main)
    {
//        source.Play();
        main.GetComponent<AudioSource>().PlayOneShot(clips[0]);
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

    /*+++++++++++++++++++++++++++++++++++++++++++++
     |      攻撃後の座標変更
     +++++++++++++++++++++++++++++++++++++++++++++*/
    [Command]
    void CmdResetPlayer(GameObject main, GameObject opponent, Vector3 attacker, Vector3 receiver)
    {
        main.transform.position = attacker;
        opponent.transform.position = receiver;
    }


    /*+++++++++++++++++++++++++++++++++++++++++++++
     |      プレイヤーソナー
     +++++++++++++++++++++++++++++++++++++++++++++*/
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
        Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10, new Color(1.0f, 0.0f, 0.0f, 1.0f));
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
        Plane.GetComponent<SimpleSonarShader_Object>().StartSonarRing(point, impulse * 10, new Color(0.0f, 0.0f, 1.0f, 1.0f));
    }
}