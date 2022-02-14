using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyReady : MonoBehaviour
{
    public GameObject rdPlayer;
    public GameObject coPlayer;
    GameObject SystemObj;
    int cPlayer;
    int rPlayer;

    // Start is called before the first frame update
    void Start()
    {
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


}
