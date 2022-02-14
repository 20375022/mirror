using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TimerController : NetworkBehaviour
{
    public float totalTime;
    public int tim;
    [SyncVar]
    public int seconds;

    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

        TimeInc();
        TimeSync();
    }

    [ServerCallback]
    void TimeInc()
    {
        totalTime += Time.deltaTime;
        seconds = (int)totalTime;
        tim = seconds;
    }

    [ClientCallback]
    void TimeSync()
    {
        tim = GetComponent<TimerController>().seconds;
    }

}

