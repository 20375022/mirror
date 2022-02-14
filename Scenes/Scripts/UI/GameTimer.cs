using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    GameObject timer;
    public int Time;


    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");
    }

    // Update is called once per frame
    void Update()
    {
        Time = timer.gameObject.GetComponent<TimerController>().seconds;
        this.GetComponent<Text>().text = Time.ToString();
    }
}
