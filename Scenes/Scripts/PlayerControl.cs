using UnityEngine;
using Mirror;

public class PlayerControl : NetworkBehaviour
{
    // 定期更新時に呼ばれる
    void FixedUpdate()
    {
        // ローカルプレイヤーの時
        if (isLocalPlayer)
        {
            CmdMoveSphere();
        }
    }

    // 球の移動
    void CmdMoveSphere()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0f, 0f, 0.1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0f, 0f, -0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-0.1f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0.1f, 0f, 0f);
        }
    }
}