using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{

    // Animator コンポーネント
    private Animator animator;

    // 設定したフラグの名前
    private const string key_isRun = "isRun";
    private const string key_isJump = "isJump";

    // 初期化メソッド
    void Start()
    {
        // 自分に設定されているAnimatorコンポーネントを習得する
        this.animator = GetComponent<Animator>();
    }

    // 1フレームに1回コールされる
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            // 移動ボタンを押下している
            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)) ||
                 (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
            {
                // WaitからRunに遷移する
                this.animator.SetBool(key_isRun, true);
            }
            else
            {
                // RunからWaitに遷移する
                this.animator.SetBool(key_isRun, false);
            }
        }

    }
}