using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeTImer : MonoBehaviour
{
    GameObject SysObj;
    int Time;

    // Start is called before the first frame update
    void Start()
    {
        SysObj = GameObject.FindGameObjectWithTag("System");
    }

    // Update is called once per frame
    void Update()
    {
        Time = SysObj.gameObject.GetComponent<GameSystemManage>().escapeTime;
        this.GetComponent<Text>().text = Time.ToString();
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
