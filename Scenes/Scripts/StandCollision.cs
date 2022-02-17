using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandCollision : MonoBehaviour
{
    public PlayerControl PLYCON;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "yuka")
        {
            PLYCON.StandCollisionHit();
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "yuka")
        {
            PLYCON.StandCollisionExit();
        }
    }

}
