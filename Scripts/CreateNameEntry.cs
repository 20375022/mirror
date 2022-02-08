using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CreateNameEntry : NetworkBehaviour
{
    //
    public static GameObject NameEntryes;
    //UI
    public static GameObject Gameui;
    //
    public static GameObject cam;
    //右下に配置するボタン
    public GameObject PlayerBtn;
    //メイン操作領域
    public GameObject TapArea;
    //スタート時点のカメラ
    public GameObject StartCam;
    //プレイヤーカメラ
    public GameObject PlyCam;
    //ステージ
    public GameObject Stage;
    //
    public GameObject NameEntry;

    // Start is called before the first frame update
    void Start()
    {
        //後に有効化するために取得
        NameEntryes = GameObject.Find("Namegeter");         //入力欄を取得
        //後で使用するため無効化
        NameEntryes.SetActive(false);
    }

    public void PlayerEntryInit()
    {
        //ステージ生成
        Instantiate(Stage);
        //既存カメラを削除
        Destroy(StartCam);
        Destroy(NameEntry);

        //ローカルプレイヤーの重力を有効化
        NetworkClient.localPlayer.gameObject.GetComponent<Rigidbody>().useGravity = true;

        //プレイヤーUIを設定
        Instantiate(PlyCam);
        Instantiate(PlayerBtn);
        Instantiate(TapArea);
    }

    //名前入力欄を復活
    public static void ResetName()
    {
        NameControle.InputF = 0;
        NameEntryes.SetActive(true);
    }
}
