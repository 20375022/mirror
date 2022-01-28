using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{

    // Animator コンポーネント
    private Animator animator;
    [SyncVar]
    public int PlyObj;

    // 設定したフラグの名前
    private const string key_isRun = "isRun";
    private const string key_isJump = "isJump";

    // 初期化メソッド
    void Start()
    {
        // 自分に設定されているAnimatorコンポーネントを取得する
        animator = this.transform.GetChild(PlyObj).GetComponent<Animator>();
    }

    // 1フレームに1回コールされる
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            // 移動ボタンを押下している
            //           if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)) ||
            //               (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // WalkからRunに遷移する
                animator.SetBool(key_isRun, true);
            }
            else
            {
                // RunからWalkに遷移する
                this.animator.SetBool(key_isRun, false);
            }
        }

    }
}