using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class PlayerControl1 : NetworkBehaviour
{
    
    //HPバー
    public Slider PHpbar;
    //ミニHPバー
    public Slider Hpbar;
    //カメラ
    Camera cam;

    //アニメーター
    private Animator animator; 

    //キー入力フラグ
    public bool SkillNKey = false;  //通常攻撃の入力キー
    public bool SkillAKey = false;  //スキルAの入力キー
    public bool SkillBKey = false;  //スキルBの入力キー
    public bool SkillCKey = false;  //スキルCの入力キー

    //攻撃識別番号
    const int normal = 1;   //通常攻撃
    const int skillA = 2;   //A攻撃
    const int skillB = 3;   //B攻撃
    const int skillC = 4;   //C攻撃
    
    //スキルごとのヒットボックス
    public GameObject HitBoxN;
    public GameObject HitBoxA;
    public GameObject HitBoxB;
    public GameObject HitBoxC;

    const int MAXHP = 200;    //最大体力値
    [SyncVar]
    public int HP;   //キャラクター体力

    const int MAXCT = 7;    //最大クールタイム
    int CT = 0;    //クールタイム

    //テクニックタイプのタグ管理
    const string TECN = "techniqueN";
    const string TECA = "techniqueA";
    const string TECB = "techniqueB";
    const string TECC = "techniqueC";

    //パワータイプのタグ管理
    const string POWN = "powerN";
    const string POWA = "powerA";
    const string POWB = "powerB";
    const string POWC = "powerC";

    //スピードタイプのタグ管理
    const string SPDN = "speedN";
    const string SPDA = "speedA";
    const string SPDB = "speedB";
    const string SPDC = "speedC";

    //スキルごとのダメージ
    const int POWNDMG = 5;
    const int POWADMG = 5;
    const int POWBDMG = 5;
    const int POWCDMG = 5;

    const int SPDNDMG = 5;
    const int SPDADMG = 5;
    const int SPDBDMG = 5;
    const int SPDCDMG = 5;

    const int TECNDMG = 5;
    const int TECADMG = 5;
    const int TECBDMG = 5;
    const int TECCDMG = 5;

    void Start() 
    {
        //自身でない場合はreturnを送る
        if (!isLocalPlayer) return;

        //自身のアニメーターを取得
        animator = GetComponent<Animator>();

        //ローカル環境にあるカメラを検索
        cam = Camera.main.gameObject.GetComponent<Camera>();

        //HPバーを取得
        PHpbar = GameObject.Find("ConnectUI/Canvas/Phpbar").GetComponent<Slider>();   
        if (PHpbar == null)
        {
            Debug.Log("ない");
        }

        //体力値を代入
        HP = MAXHP;
        //HPbarを初期化
        PHpbar.value = HP;
        PHpbar.maxValue = HP;
        //ミニHPbarを初期化
        Hpbar.value = HP;
        Hpbar.maxValue = HP;
    }

    // Update is called once per frame
    void Update()
    {
        //ミニ体力ゲージと体力を同期
        Hpbar.value = HP;

        if (isLocalPlayer)  //自身の操作のみ
        {                
            ////////////////////////////////////
            //当たり判定用のオブジェクトを無効//
            ////////////////////////////////////
            Cmdcancel();
            if (isServer)
            {
                Rpccancel();
            }


            //////////////////////////////////
            //      死亡処理と体力管理      //
            //////////////////////////////////
            if (HP <= 0)
            {
                animator.SetInteger("down", 1);     //Downモーションを有効化
            }
            else
            {
                //体力ゲージと体力を同期
                PHpbar.value = HP;
            }


            ////////////////////////
            //      移動処理      //
            ////////////////////////        
/*            SkillNKey = GameObject.FindWithTag("NATK").GetComponent<TapTest>()._isTouched;  //通常攻撃入力キー    
            SkillAKey = GameObject.FindWithTag("AATK").GetComponent<TapTest>()._isTouched;  //スキルAの入力キー
            SkillBKey = GameObject.FindWithTag("BATK").GetComponent<TapTest>()._isTouched;  //スキルBの入力キー
            SkillCKey = GameObject.FindWithTag("CATK").GetComponent<TapTest>()._isTouched;  //スキルCの入力キー
   */     

            ///////////////////////////
            //        攻撃処理       //
            ///////////////////////////

            if (SkillNKey && CT == 0) //通常攻撃
            {
                //クールタイムに最大クールタイムを代入
                CT = MAXCT;
                animator.SetInteger("atkF", normal);     //攻撃モーションを有効化
                StartCoroutine(resetAtk());                //適用されるまで1f待つ
                CmdApplyN();
                StartCoroutine(cooltimeControll());
                if (isServer)
                {
                    RpcApplyN();
                }
            }

            if (SkillAKey && CT == 0) //スキルA攻撃
            {
                //クールタイムに最大クールタイムを代入
                CT = MAXCT;
                animator.SetInteger("atkF", skillA);     //攻撃モーションを有効化
                StartCoroutine(resetAtk());                //適用されるまで1f待つ
                CmdApplyA();
                StartCoroutine(cooltimeControll());
                if (isServer)
                {
                    RpcApplyA();
                }
            }

            if (SkillBKey && CT == 0) //スキルB攻撃
            {
                //クールタイムに最大クールタイムを代入
                CT = MAXCT;
                animator.SetInteger("atkF", skillB);     //攻撃モーションを有効化
                StartCoroutine(resetAtk());                //適用されるまで1f待つ
                CmdApplyB();
                StartCoroutine(cooltimeControll());
                if (isServer)
                {
                    RpcApplyB();
                }
            }

            if (SkillCKey && CT == 0) //スキルC攻撃
            {
                //クールタイムに最大クールタイムを代入
                CT = MAXCT;
                animator.SetInteger("atkF", skillC);     //攻撃モーションを有効化
                StartCoroutine(resetAtk());                //適用されるまで1f待つ
                CmdApplyC();
                StartCoroutine(cooltimeControll());
                if (isServer)
                {
                    RpcApplyC();
                }
            }
        }
    }

    //ヒット判定に入っている間
    void OnTriggerStay(Collider collision)
    {
        //自身でない場合はreturnを送る
        if (!isLocalPlayer) return;

        //////////////////////////////////////
        //        パワータイプの攻撃　　　  //
        //////////////////////////////////////
        if (collision.gameObject.tag == POWN)
        {
            dmg();                            //やられモーションを有効化
            HP -= POWNDMG;
        }
        if (collision.gameObject.tag == POWA)
        {
            dmg();                            //やられモーションを有効化
            HP -= POWADMG;
        }
        if (collision.gameObject.tag == POWB)
        {
            dmg();                            //やられモーションを有効化
            HP -= POWBDMG;
        }
        if (collision.gameObject.tag == POWC)
        {
            dmg();                            //やられモーションを有効化
            HP -= POWCDMG;
        }

        //////////////////////////////////////
        //      スピードタイプの攻撃　　 　//
        //////////////////////////////////////
        if (collision.gameObject.tag == SPDN)
        {
            dmg();                            //やられモーションを有効化
            HP -= SPDNDMG;
        }
        if (collision.gameObject.tag == SPDA)
        {
            dmg();                            //やられモーションを有効化
            HP -= SPDADMG;
        }
        if (collision.gameObject.tag == SPDB)
        {
            dmg();                            //やられモーションを有効化
            HP -= SPDBDMG;
        }
        if (collision.gameObject.tag == SPDC)
        {
            dmg();                            //やられモーションを有効化
            HP -= SPDCDMG;
        }

        //////////////////////////////////////
        //      テクニックタイプの攻撃　　　//
        //////////////////////////////////////
        if (collision.gameObject.tag == TECN)
        {
            dmg();                            //やられモーションを有効化
            HP -= TECNDMG;
        }
        if (collision.gameObject.tag == TECA)
        {
            dmg();                            //やられモーションを有効化
            HP -= TECADMG;
        }
        if (collision.gameObject.tag == TECB)
        {
            dmg();                            //やられモーションを有効化
            HP -= TECBDMG;
        }
        if (collision.gameObject.tag == TECC)
        {
            dmg();                            //やられモーションを有効化
            HP -= TECCDMG;
        }
    }

    //ヒットから外れた際の処理
    void OnTriggerExit(Collider other)
    {
    }

    //ダメージ処理
    void dmg()
    {
        animator.SetInteger("dmg", 1);     //やられモーションを有効化
        StartCoroutine(resetDmg());          //適用されるまで1f待つ
    }

    private IEnumerator cooltimeControll()
    {
        while (CT > 0)
        {
            Debug.Log(CT);
            yield return new WaitForSeconds(0.1f);  //0.1秒待つ
            CT--;
        }
        CT = 0;
    }

    private IEnumerator resetAtk()
    {
        yield return new WaitForSeconds(0.1f);  //0.1秒待つ
        animator.SetInteger("atkF", 0);     //攻撃モーションを初期化
    }
    private IEnumerator resetDmg()
    {
        yield return new WaitForSeconds(0.1f);  //0.1秒待つ
        animator.SetInteger("dmg", 0);     //やられモーションを初期化
    }


    ////<summary>
    ////コマンドたち
    ////</summary>
        
    [Command]
    void CmdApplyN()
    {
        RpcApplyN();
    }
    [ClientRpc]
    void RpcApplyN()
    {
        HitBoxN.SetActive(true);    //当たり判定用のオブジェクトを有効化
    }

    [Command]
    void CmdApplyA()
    {
        RpcApplyA();
    }
    [ClientRpc]
    void RpcApplyA()
    {
        HitBoxA.SetActive(true);    //当たり判定用のオブジェクトを有効化
    }

    [Command]
    void CmdApplyB()
    {
        RpcApplyB();
    }
    [ClientRpc]
    void RpcApplyB()
    {
        HitBoxB.SetActive(true);    //当たり判定用のオブジェクトを有効化
    }

    [Command]
    void CmdApplyC()
    {
        RpcApplyC();
    }
    [ClientRpc]
    void RpcApplyC()
    {
        HitBoxC.SetActive(true);    //当たり判定用のオブジェクトを有効化
    }

    [Command]
    void Cmdcancel()
    {
        Rpccancel();
    }
    [ClientRpc]
    void Rpccancel()
    {
        HitBoxN.SetActive(false);    //当たり判定用のオブジェクトを無効化
        HitBoxA.SetActive(false);    //当たり判定用のオブジェクトを無効化
        HitBoxB.SetActive(false);    //当たり判定用のオブジェクトを無効化
        HitBoxC.SetActive(false);    //当たり判定用のオブジェクトを無効化
    }
}






