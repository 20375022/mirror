using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GrovalConst;

public class EscapeTImer : MonoBehaviour
{
    GameObject SysObj;
    GameObject timer;
    int Time;

    // Start is called before the first frame update
    void Start()
    {
        SysObj = GameObject.FindGameObjectWithTag("System");
        timer = GameObject.FindGameObjectWithTag("Timer");
    }

    // Update is called once per frame
    void Update()
    {
        if (SysObj.gameObject.GetComponent<GameSystemManage>().gameMode == GameMode.GAME)
        {
            Time = timer.gameObject.GetComponent<TimerController>().estim;
            this.GetComponent<Text>().text = Time.ToString();
/*            Time = SysObj.gameObject.GetComponent<GameSystemManage>().escapeTime;
            this.GetComponent<Text>().text = Time.ToString();*/
            if (SysObj.gameObject.GetComponent<GameSystemManage>().Escape == true)
            {
                this.GetComponent<Text>().enabled = true;
            }
            else
            {
                this.GetComponent<Text>().enabled = false;
            }
        }
    }
}
