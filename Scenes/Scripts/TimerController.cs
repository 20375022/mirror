using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TimerController : NetworkBehaviour
{
    public Text timerText;
    public float totalTime;
    [SyncVar]
    int seconds;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        int tim;

        tim = TimeInc();
        tim = TimeSync();
        timerText.text = tim.ToString();
    }

    [ServerCallback]
    int TimeInc()
    {
        totalTime -= Time.deltaTime;
        seconds = (int)totalTime;

        return seconds;
    }

    [ClientCallback]
    int TimeSync()
    {
        int tim;

        tim = GetComponent<TimerController>().seconds;
        return tim;
    }

}

