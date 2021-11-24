using UnityEngine;
using Mirror;

public class PlayerControl : NetworkBehaviour
{
    public Camera mycam;

    void start()
    {
    }

    // 定期更新時に呼ばれる
    void FixedUpdate()
    {
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
            // 移動操作
            if (Input.GetKey(KeyCode.W))
            {
                z = 0.1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                z = -0.1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                x = -0.1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                x = 0.1f;
            }
            CmdMoveSphere(x, z);

            // カメラ操作 -- キーボード入力による回転処理(横)
            var view_sensitivity = 1.5f;
            if (Input.GetKey("left"))
            {
                view_sensitivity *= -1;
                transform.Rotate(0, view_sensitivity * 1f, 0);
            }
            if (Input.GetKey("right"))
            {
                transform.Rotate(0, view_sensitivity * 1f, 0);
            }
        }
    }

    // 球の移動
    [Command]
    void CmdMoveSphere(float x, float z) 
    {
        transform.Translate(x, 0f, z);
    }

}