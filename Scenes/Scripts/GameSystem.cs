using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSystem : NetworkBehaviour
{
    public GameObject Player2;
    GameObject m_MyInstantiated;

   // プレイヤーのタッチ
    [Command]
    public void 
    CmdTouchPlayer(GameObject tPlayer)
    {
        Destroy(tPlayer.gameObject, 1.0f);
    }
}


