using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyReady : MonoBehaviour
{
    bool isReady;
    public GameObject rdPlayer;
    public GameObject coPlayer;
    GameObject SystemObj;
    int cPlayer;
    int rPlayer;

    // Start is called before the first frame update
    void Start()
    {
        isReady = false;
        SystemObj = GameObject.FindGameObjectWithTag("System");
    }

    // Update is called once per frame
    void Update()
    {
        cPlayer = SystemObj.gameObject.GetComponent<GameSystemManage>().countPlayer;
        rPlayer = SystemObj.gameObject.GetComponent<GameSystemManage>().readyPlayer;
        rdPlayer.GetComponent<Text>().text = rPlayer.ToString();
        coPlayer.GetComponent<Text>().text = cPlayer.ToString();
    }


    public void OnClickReady()
    {
        if (isReady == false)
        {
            isReady = true;
            //SystemObj.GetComponent<GameSystemManage>().readyPlayer++;
            SystemObj.GetComponent<GameSystemManage>().ReadySync();
        }
        else
        {
            isReady = false;
            SystemObj.GetComponent<GameSystemManage>().readyPlayer--;
        }
    }

}
