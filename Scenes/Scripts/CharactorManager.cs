using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public enum PlayerType
{
    piesu = 0,
    pierun,
    killsu,
    killrun,
    none
}

public enum KillerType
{
    killer = 0,
    piero
}

public enum SurvivorType
{
    runner = 0,
    sutai
}



//CharactorManager.playerType
public class CharactorManager : NetworkBehaviour
{
    public GameObject networkPanel;
    public GameObject selectPanel;
    public static PlayerType playerType;
    public static KillerType killerType;
    public static SurvivorType survivorType;
    public GameObject Enteradress;

    // Start is called before the first frame update
    void Start()
    {
        networkPanel.SetActive(true);
        selectPanel.SetActive(false);
        playerType = PlayerType.none;
        killerType = KillerType.killer;
        survivorType = SurvivorType.runner;
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
            selectPanel.SetActive(true);
        }
        else
        {
            DisabledBtn();
        }
    }

    //ボタンを無効化
    void DisabledBtn() 
    {
        selectPanel.SetActive(false);
        Cmdplytypechg(playerType);
    }

    //Killerのモデル選択(左ボタン)
    public void OnKillerPlus()
    {
        Debug.Log("キラー +");
    }
    //Killerのモデル選択(右ボタン)
    public void OnKillerMinus()
    {
        Debug.Log("キラー -");
    }

    //Survivorのモデル選択(左ボタン)
    public void OnSurvivorPlus()
    {
        Debug.Log("サバイバー +");

    }
    //Survivorのモデル選択(右ボタン)
    public void OnSurvivorMinus()
    {
        Debug.Log("サバイバー -");
    }


    /*
        //テクニカルタイプ選択
        public void SelectTechniquetype()
        {
            Debug.Log("technique");
            playerType = PlayerType.tec;
    //        Cmdplytypechg(playerType);
            DisabledBtn();
        }*/

    [Command]
    void Cmdplytypechg(PlayerType pt)
    {
        Debug.Log("yobareta");
        playerType = pt;
    }
}






