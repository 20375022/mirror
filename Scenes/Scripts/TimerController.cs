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

    [HideInInspector]
    public float estotalTime;
    [HideInInspector]
    public int estim;
    [SyncVar]
    public int esseconds;



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


    [ServerCallback]
    public void esTimeInc()
    {
        estotalTime -= Time.deltaTime;
        esseconds = (int)estotalTime;
        estim = esseconds;
    }

    [ClientCallback]
    public void esTimeSync()
    {
        estim = GetComponent<TimerController>().esseconds;
    }

}

