using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public bool Darkstart;
//    TransitionDark Dark = GameObject.Find("brack").GetComponent<TransitionDark>();

    void Start() {
        Darkstart = false;
//        Debug.Log("TStart");
    }

    void Update()
    {
        Debug.Log("push" + Darkstart);
//        Debug.Log("TUpdate");
    }

    public void OnClickStart()
    {
        Darkstart = true;
    }

    public void BeginGame() {
        SceneManager.LoadScene("game");
    }

}
