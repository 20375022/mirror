﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCollision : MonoBehaviour
{
    public PlayerControl PLYCON;


    void Update()
    {
        if (PLYCON.Killerflg == true)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PLYCON.MainCollisionHit(other);
        }
    }
}
