using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCollision : MonoBehaviour
{
    public PlayerControl PLYCON;


    void Update()
    {
        if (PLYCON.Killerflg == true)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Killer")
        {
            PLYCON.GunCollisionHit(other);
        }
    }
}
