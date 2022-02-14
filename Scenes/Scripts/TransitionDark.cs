using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionDark : MonoBehaviour
{
    [SerializeField]
    private Material _transitionIn;
    Title Startmanager;
    bool Indark = false;
    public GameObject Titlemanager;
    float current;

    void Start()
    {
//        Debug.Log("DStart");
        current = 0;
        Startmanager = Titlemanager.GetComponent<Title>() ;
    }

    void Update() {
//        Debug.Log("DUpdate");
        Debug.Log("flg" + Startmanager.Darkstart);
        if (Startmanager.Darkstart == true && Indark == false)
        {
            Indark = true;
            StartCoroutine(BeginTransition());
        }
    }

    IEnumerator BeginTransition()
    {
        yield return Animate(_transitionIn, 1);
    }

    /// <summary>
    /// time秒かけてトランジションを行う
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Animate(Material material, float time)
    {
//        GetComponent<Image>().material = material;
        while (current < time)
        {
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 1);
        Startmanager.BeginGame();
    }
}
