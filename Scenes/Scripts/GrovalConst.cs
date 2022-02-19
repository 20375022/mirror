using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GrovalConst
{
    /// <summary>
    /// 共通定数
    /// </summary>
    public static class Const
    {
        public const int    MAX_PLAYER  = 2;    // ゲームの人数 

        public const float  SPEED_WALK  = 5f;   // 歩く速さ
        public const float  SPEED_RUN   = 8f;   // 走る速さ

        public const float  START_TIME  = 4f;   // ロビーからゲームスタート 
        public const float  GAME_TIME   = 26f; // ゲームの時間(タイマーの性質上 + 1秒 で始める) 180
        public const float  ESCAPE_TIME = 11f;  // 集合してから勝つまで
    }

    // ゲームのモード
    public enum GameMode
    {
        LOBBY = 0,  // ロビー
        GAME,       // ゲーム中
        RESULT      // リザルト画面(シーン移行？)
    }

    // プレイヤーのモード(状態)
    public enum PlayerMode  
    {
        WAIT = 0,   // 立ちモード
        WALK,       // 歩くモード
        RUN,        // 走るモード
        ATK,        // 攻撃モード
        FALL,       // 落ちてるモード
        LANDING,    // 着地したモード
        STUN
    }

}
