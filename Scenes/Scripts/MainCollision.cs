using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCollision : MonoBehaviour
{
    public PlayerControl PLYCON;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PLYCON.MainCollisionHit(other);
        }
    }
}
