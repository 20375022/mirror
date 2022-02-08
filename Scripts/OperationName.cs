﻿
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OperationName : MonoBehaviour
{
    private GameObject namePlate;	//　名前を表示しているプレート
    public Text nameText;	//　名前を表示するテキスト    

    void Start()
    {
        namePlate = nameText.transform.parent.gameObject;
    }

    void LateUpdate()
    {
        namePlate.transform.position = this.transform.position;
        namePlate.transform.rotation = Camera.main.transform.rotation;
    }

}
