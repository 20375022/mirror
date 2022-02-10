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
    int killerType;
    int survivorType;
    public GameObject SelectKillerModel;
    public GameObject SelectSurvivorModel;
    public GameObject Enteradress;

    // Start is called before the first frame update
    void Start()
    {
        networkPanel.SetActive(true);
        selectPanel.SetActive(false);
        playerType = PlayerType.none;
        killerType = (int)KillerType.killer;
        survivorType = (int)SurvivorType.runner;
        SelectKillerModel.transform.GetChild(killerType).gameObject.SetActive(true);
        SelectSurvivorModel.transform.GetChild(survivorType).gameObject.SetActive(true);
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
            networkPanel.SetActive(false);
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
        Destroy(SelectKillerModel);
        Destroy(SelectSurvivorModel);
        Cmdplytypechg(playerType);
    }

    //Killerのモデル選択(左ボタン)
    public void OnKillerPlus()
    {
        Debug.Log("キラー +");
        SelectKillerModel.transform.GetChild(killerType).gameObject.SetActive(false);
        if (killerType == (int)KillerType.piero)
        {
            killerType = (int)KillerType.killer;
        }
        else
        {
            killerType++;
        }
        SelectKillerModel.transform.GetChild(killerType).gameObject.SetActive(true);
    }
    //Killerのモデル選択(右ボタン)
    public void OnKillerMinus()
    {
        Debug.Log("キラー -");
        SelectKillerModel.transform.GetChild(killerType).gameObject.SetActive(false);
        if (killerType == (int)KillerType.killer)
        {
            killerType = (int)KillerType.piero;
        }
        else
        {
            killerType--;
        }
        SelectKillerModel.transform.GetChild(killerType).gameObject.SetActive(true);
    }

    //Survivorのモデル選択(左ボタン)
    public void OnSurvivorPlus()
    {
        Debug.Log("サバイバー +");
        SelectSurvivorModel.transform.GetChild(survivorType).gameObject.SetActive(false);
        if (survivorType == (int)SurvivorType.sutai)
        {
            survivorType = (int)SurvivorType.runner;
        }
        else
        {
            survivorType++;
        }
        SelectSurvivorModel.transform.GetChild(survivorType).gameObject.SetActive(true);
    }
    //Survivorのモデル選択(右ボタン)
    public void OnSurvivorMinus()
    {
        Debug.Log("サバイバー -");
        SelectSurvivorModel.transform.GetChild(survivorType).gameObject.SetActive(false);
        if (survivorType == (int)SurvivorType.runner)
        {
            survivorType = (int)SurvivorType.sutai;
        }
        else
        {
            survivorType--;
        }
        SelectSurvivorModel.transform.GetChild(survivorType).gameObject.SetActive(true);
    }



    //テクニカルタイプ選択
    public void SelectedModel()
    {
        Debug.Log("Selected");
        switch (killerType)
        {
            case (int)KillerType.killer:
                switch (survivorType)
                {
                    case(int)SurvivorType.runner:
                        Debug.Log("killrun");
                        playerType = PlayerType.killrun;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.sutai:
                        Debug.Log("killsu");
                        playerType = PlayerType.killsu;
                        DisabledBtn();
                        break;

//                    case (int)SurvivorType:
//                        break;
                }
                break;

            case (int)KillerType.piero:
                switch (survivorType)
                {
                    case(int)SurvivorType.runner:
                        Debug.Log("pierun");
                        playerType = PlayerType.pierun;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.sutai:
                        Debug.Log("piesu");
                        playerType = PlayerType.piesu;
                        DisabledBtn();
                        break;

//                    case (int)SurvivorType:
//                        break;
                }
                break;

//            case (int)KillerType:
//                break;
        }
    }

    [Command]
    void Cmdplytypechg(PlayerType pt)
    {
        Debug.Log("yobareta");
        playerType = pt;
    }
}






