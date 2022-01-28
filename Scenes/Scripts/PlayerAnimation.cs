using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{

    // Animator コンポーネント
    private Animator animator;
    public PlayerControl playerControl;
    int PlyObj;

    // 設定したフラグの名前
    private const string key_isRun = "isRun";
    private const string key_isJump = "isJump";

    // 初期化メソッド
    void Start()
    {
        PlyObj = playerControl.PlyObj;
        // 自分に設定されているAnimatorコンポーネントを取得する
        //        this.animator = this.transform.GetChild(PlyObj).GetComponent<Animator>();
        this.animator = GetComponent<Animator>();
    }

    public void PlyRunAnim()
    {
        this.animator.SetBool(key_isRun, true);
    }

    public void PlyWalkAnim()
    {
        this.animator.SetBool(key_isRun, false);
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
                //this.animator.SetBool(key_isRun, true);
            }
            else
            {
                // RunからWalkに遷移する
                //this.animator.SetBool(key_isRun, false);
            }
        }

    }
}