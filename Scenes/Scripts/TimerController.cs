using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using GrovalConst;

public class TimerController : NetworkBehaviour
{
    [HideInInspector]
    public float totalTime;
    [HideInInspector]
    public int tim;
    [SyncVar]
    public int seconds;



    /// <summary>
    /// ゲーム中
    /// </summary>
    [ServerCallback]
    public void TimeInc()
    {
        totalTime -= Time.deltaTime;
        seconds = (int)totalTime;
        tim = seconds;
    }

    [ClientCallback]
    public void TimeSync()
    {
        tim = GetComponent<TimerController>().seconds;
    }

}

