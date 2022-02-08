using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class NameControle : NetworkBehaviour
{
    //プレイヤー自身
    public GameObject Player;
    public Text nameText;	//　名前を表示するテキスト
    public static Text Dispname;
    public static int InputF = Nonattached;
    private const int Nonattached = 99;
    private const int EntryName = 1;
    private const int UpdateName = 2;

    void Start()
    {
       
    }

    //表示する方
    void Update()
    {
        //自分以外のプレイヤーには適用しない
        if (!isLocalPlayer) return;

        //接続されていてかつDispnameが定義されていない場合
        if (Nonattached == InputF)
        {
            //名前入力欄を呼び出す
            CreateNameEntry.ResetName();
            //インプットフィールドを変数に格納する
            Dispname = GameObject.Find("NameEntry/Text").GetComponent<Text>();
            //フラグを初期化
            InputF = 0;
        }

        //文字列が入力されていて、ボタンが押された場合
        if (InputF == EntryName)  //自身の操作のみ
        {            
            //プレイヤーに適用
            nameText.text = Dispname.text;

            //左上表示にも適用
            GameObject.Find("Canvas/PnameDis").GetComponent<Text>().text = Dispname.text;

            //プレイヤーを検索し取得
            Player = NetworkClient.localPlayer.gameObject;

            //名前変更を共有
//            CmdChangeName((string)Dispname.text);

            //無効化
            CreateNameEntry.NameEntryes.SetActive(false);

            //名前を送信し続けるモードに移行
            InputF = UpdateName;
        }

        //入力されたあとに名前を送信し続ける(途中参加者にも表示する)
        if (InputF == UpdateName)
        {
            CmdChangeName((string)Dispname.text);
            if (isServer)
            {
                RpcChangeName((string)Dispname.text);
            }
        }
    }

    //決定ボタンが押された際に呼び出して入力
    public void Inputtext()
    {
        if (Dispname.text.Length == 0)
        {   
            return;
        }
        InputF = EntryName;     //InputFの値を変更
    }

    //全員名前変更を共有
    [Command]
    void CmdChangeName(string AfterName)
    {
        RpcChangeName(AfterName);
    }

    [ClientRpc]
    void RpcChangeName(string AfterName)
    {
        //プレイヤーのオブジェクト
        GetComponentInChildren<Text>().text = AfterName;
    }
}


