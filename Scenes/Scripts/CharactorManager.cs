using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public enum PlayerType
{
    killgaki = 0,
    killrun,
    killOL,
    piegaki,
    pierun,
    pieOL,
    pastgaki,
    pastrun,
    pastOL,
    none
}

public enum KillerType
{
    killer = 0,
    piero,
    past
}

public enum SurvivorType
{
    gaki = 0,
    runner,
    OL
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
        survivorType = (int)SurvivorType.gaki;
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
    }

    //ボタンを無効化
    void DisabledBtn() 
    {
        selectPanel.SetActive(false);
        Destroy(SelectKillerModel);
        Destroy(SelectSurvivorModel);
        plytypechg(playerType);
    }

    //Killerのモデル選択(左ボタン)
    public void OnKillerPlus()
    {
        Debug.Log("キラー +");
        SelectKillerModel.transform.GetChild(killerType).gameObject.SetActive(false);
        if (killerType == (int)KillerType.past)
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
            killerType = (int)KillerType.past;
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
        if (survivorType == (int)SurvivorType.OL)
        {
            survivorType = (int)SurvivorType.gaki;
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
        if (survivorType == (int)SurvivorType.gaki)
        {
            survivorType = (int)SurvivorType.OL;
        }
        else
        {
            survivorType--;
        }
        SelectSurvivorModel.transform.GetChild(survivorType).gameObject.SetActive(true);
    }



    //キャラ選択
    public void SelectedModel()
    {
        Debug.Log("Selected");
        switch (killerType)
        {
            case (int)KillerType.killer:
                switch (survivorType)
                {
                    case(int)SurvivorType.gaki:
                        Debug.Log("killgaki");
                        playerType = PlayerType.killgaki;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.runner:
                        Debug.Log("killrun");
                        playerType = PlayerType.killrun;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.OL:
                        Debug.Log("killOL");
                        playerType = PlayerType.killOL;
                        DisabledBtn();
                        break;
                }
                break;

            case (int)KillerType.piero:
                switch (survivorType)
                {
                    case(int)SurvivorType.gaki:
                        Debug.Log("piegaki");
                        playerType = PlayerType.piegaki;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.runner:
                        Debug.Log("pierun");
                        playerType = PlayerType.pierun;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.OL:
                        Debug.Log("pieOL");
                        playerType = PlayerType.pieOL;
                        DisabledBtn();
                        break;
                }
                break;

            case (int)KillerType.past:
                switch (survivorType)
                {
                    case (int)SurvivorType.gaki:
                        Debug.Log("pastgaki");
                        playerType = PlayerType.pastgaki;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.runner:
                        Debug.Log("pastrun");
                        playerType = PlayerType.pastrun;
                        DisabledBtn();
                        break;

                    case (int)SurvivorType.OL:
                        Debug.Log("pastOL");
                        playerType = PlayerType.pastOL;
                        DisabledBtn();
                        break;
                }
                break;
        }

    }

 
    void plytypechg(PlayerType pt)
    {
        Debug.Log("yobareta");
        playerType = pt;
    }
}






