using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{

    // Animator コンポーネント
    private Animator animator;
    public PlayerControl playerControl;
    int PlyObj;

    // 設定したフラグの名前
    private const string key_isWalk = "isWalk";
    private const string key_isRun = "isRun";

    // 初期化メソッド
    void Start()
    {
        PlyObj = playerControl.PlyObj;
        // 自分に設定されているAnimatorコンポーネントを取得する
        //this.animator = this.transform.GetChild(PlyObj).GetComponent<Animator>();
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
                // WaitからWalkに遷移する
                this.animator.SetBool(key_isWalk, true);

                // 歩いている間は走りに移行できる
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // WalkからRunに遷移する
                    this.animator.SetBool(key_isRun, true);
                }
                else
                {
                    // RunからWalkに遷移する
                    this.animator.SetBool(key_isRun, false);
                }
            }
            else
            {
                // WalkからWaitに遷移する
                this.animator.SetBool(key_isWalk, false);
                this.animator.SetBool(key_isRun, false);
            }

        }

    }
}