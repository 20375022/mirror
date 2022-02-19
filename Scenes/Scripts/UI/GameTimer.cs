using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GrovalConst;

public class GameTimer : MonoBehaviour
{
    GameObject timer;
    GameObject Sys;
    int Time;


    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");
        Sys = GameObject.FindGameObjectWithTag("System");
    }

    // Update is called once per frame
    void Update()
    {
        switch (Sys.gameObject.GetComponent<GameSystemManage>().gameMode)
        {
            case GameMode.LOBBY:
                if (Sys.gameObject.GetComponent<GameSystemManage>().Startflg == true)
                {
                    this.GetComponent<Text>().enabled = true;
                    Time = timer.gameObject.GetComponent<TimerController>().tim;
                    this.GetComponent<Text>().text = Time.ToString();
                }
                else
                {
                    this.GetComponent<Text>().enabled = false;
                }
                break;

            case GameMode.GAME:
                Time = timer.gameObject.GetComponent<TimerController>().tim;
                this.GetComponent<Text>().text = Time.ToString();
                break;
        }
    }
}
