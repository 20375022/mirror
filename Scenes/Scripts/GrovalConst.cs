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
        public const float  SPEED_WALK  = 5f;   // 歩く速さ
        public const float  SPEED_RUN   = 8f;   // 走る速さ

        public const float  START_TIME  = 4f;   // ロビーからゲームスタート 
        public const float  GAME_TIME   = 10f;  // ゲームの時間
    }

    public enum GameMode
    {
        LOBBY = 0,
        GAME,
        RESULT
    }


}
