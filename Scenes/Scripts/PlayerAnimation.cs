using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{

    // Animator コンポーネント
    private Animator animator;
    public PlayerControl playerControl;
    
    // プレイヤーの状態
    

    // 設定したフラグの名前
    private const string key_isONI = "isONI";
    private const string key_isWalk = "isWalk";
    private const string key_isRun = "isRun";
    private const string key_isNorikoe = "isNorikoe";
    bool norikoe;

    // 初期化メソッド
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // 1フレームに1回コールされる
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            norikoe = animator.GetBool("isNorikoe");

            // 鬼であるかずっと確認する
            if (playerControl.Killerflg == true)
            {
                // 逃げアニメーションから鬼アニメーションに遷移する
                this.animator.SetBool(key_isONI, true);
            }
            else
            {
                // 鬼アニメーションから逃げアニメーションに遷移する
                this.animator.SetBool(key_isONI, false);
            }

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

            // スペースで乗り越え
            if (norikoe == false)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    this.animator.SetBool(key_isNorikoe, true);
                }
                if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    Debug.Log("乗り越え終了");
            }
            else 
            {
                this.animator.SetBool(key_isNorikoe, false);
            }

        }

    }
}