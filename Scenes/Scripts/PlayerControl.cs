using UnityEngine;
using Mirror;

public class PlayerControl : NetworkBehaviour
{
    public Camera mycam;
    public GameObject cam;
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;

    float moveSpeed = 3f;

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    // 定期更新時に呼ばれる
    void FixedUpdate()
    {
        rb = this.transform.GetComponent<Rigidbody>();
        float x = 0.0f;
        float z = 0.0f;

        // カメラの有効化(自分の以外は無効に)
        if (!isLocalPlayer)
        {
            mycam.enabled = false;
        }
        else
        {
            mycam.enabled = true;
        }

        // ローカルプレイヤーの時
        if (isLocalPlayer) {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + cam.transform.right * inputHorizontal;

            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            var changed = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
            CmdMoveSphere(changed);

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                var rot = Quaternion.LookRotation(moveForward);
                CmdRotatePlayer(rot);
            }
            //// 移動
            //if (Input.GetKey(KeyCode.W))
            //{
            //    z = 0.1f;
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    z = -0.1f;
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    x = -0.1f;
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    x = 0.1f;
            //}
            //CmdMoveSphere(x, z);
        }
    }

    // 球の移動
    [Command]
    void CmdRotatePlayer(Quaternion rotate)
    {
        transform.rotation = rotate ;
    }

    [Command]
    void CmdMoveSphere(Vector3 move)
    {
        GetComponent<Rigidbody>().velocity = move;
    }

}