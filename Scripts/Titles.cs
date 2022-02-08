using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.UI; 

public class Titles : MonoBehaviour
{
    public GameObject Canvas;

    //シーン移行
    public void EnterGame()
    {
        //シーン移動
        SceneManager.LoadScene("UITest");
    }
}
