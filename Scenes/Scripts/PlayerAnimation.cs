﻿using UnityEngine;
using Mirror;
using GrovalConst;

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

            // plyModeでモード毎にアニメーション移行
            switch (playerControl.plyMode)
            {
                case PlayerMode.WALK:       // 歩きに移行する
                    this.animator.SetBool(key_isRun, false);
                    this.animator.SetBool(key_isWalk, true);
                    break;

                case PlayerMode.RUN:        // 走りに移行する
                    this.animator.SetBool(key_isRun, true);
                    break;

                case PlayerMode.WAIT:
                    this.animator.SetBool(key_isWalk, false);
                    this.animator.SetBool(key_isRun, false);
                    break;
            }
        }
    }
}