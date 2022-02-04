using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    [SerializeField] GameObject select;
    [SerializeField] GameObject network;

    void Start()
    {
        select.SetActive(true);
    }

    public void OnKillerButton()
    {
        Debug.Log("killer おした");
        select.SetActive(false);
        network.SetActive(true);
    }
    public void OnrunnerButton()
    {
        Debug.Log("runner おした");
        select.SetActive(false);
        network.SetActive(true);
    }

    public void OnClickBackButton()
    {
        select.SetActive(true);
        network.SetActive(false);
    }



}



