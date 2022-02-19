using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandCollision : MonoBehaviour
{
    public PlayerControl PLYCON;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "yuka")
        {
            PLYCON.LandCollisionHit();
        }
    }
}


