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
    public bool Darkstart;
    bool Indark = false;
    public GameObject Titlemanager;
    public Material mat;

    void Start()
    {
        mat.SetFloat("_Alpha", 0);
        Startmanager = Titlemanager.GetComponent<Title>();
    }

    void Update()
    {
        //        Debug.Log("flg" + Startmanager.Darkstart);
        if (Startmanager.Darkstart == true)
        {
            if (Indark == false)
            {
                Indark = true;
                StartCoroutine(BeginTransition());
            }
        }
        else
        {
            mat.SetFloat("_Alpha", 0);
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
        float current = 0;
        GetComponent<Image>().material = material;
        while (current < time)
        {
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        material.SetFloat("_Alpha", 1);
        yield return new WaitForSeconds(1.2f);
        Startmanager.BeginGame();
    }
}
