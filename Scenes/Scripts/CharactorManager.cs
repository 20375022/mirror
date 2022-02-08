using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public enum PlayerType
{
    power = 0,
    speed,
    tec,
    none
}

//CharactorManager.playerType
public class CharactorManager : NetworkBehaviour
{
    public GameObject powerbtn;
    public GameObject speedbtn;
    public GameObject tecbtn;
    public GameObject Net;
    public static PlayerType playerType;
    public GameObject Enteradress;

    // Start is called before the first frame update
    void Start()
    {
        playerType = PlayerType.none;
        powerbtn.SetActive(false);
        speedbtn.SetActive(false);
        tecbtn.SetActive(false);
        //Net.SetActive(false);
    }

    //ボタンを有効化
    public void EnabledBtnCheck()
    {
        //短縮用
        Text InputIP = Enteradress.GetComponent<Text>();

        //9文字以下ならreturn;
        if (InputIP.text.Length < 9) return;

        EnableBtn();
    }

    public void EnableBtn() 
    {
        if (playerType == PlayerType.none)
        {
            powerbtn.SetActive(true);
            speedbtn.SetActive(true);
            tecbtn.SetActive(true);
        }
        else
        {
            DisabledBtn();
        }
    }

    //ボタンを無効化
    void DisabledBtn() 
    {
        powerbtn.SetActive(false);
        speedbtn.SetActive(false);
        tecbtn.SetActive(false);
        //ネットワークを有効化
        //Net.SetActive(true);
        Cmdplytypechg(playerType);
    }

    //パワータイプ選択
    public void SelectPowertype()
    {
        Debug.Log("power");
        playerType = PlayerType.power;
//        Cmdplytypechg(playerType);
        DisabledBtn();
    }


    //スピードタイプ選択
    public void SelectSpeedtype()
    {
        Debug.Log("speed");
        playerType = PlayerType.speed;
//        Cmdplytypechg(playerType);
        DisabledBtn();
    }

    //テクニカルタイプ選択
    public void SelectTechniquetype()
    {
        Debug.Log("technique");
        playerType = PlayerType.tec;
//        Cmdplytypechg(playerType);
        DisabledBtn();
    }

    [Command]
    void Cmdplytypechg(PlayerType pt)
    {
        Debug.Log("yobareta");
        playerType = pt;
    }
}






