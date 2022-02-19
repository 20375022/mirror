using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandCollision : MonoBehaviour
{
    public PlayerControl PLYCON;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "yuka")
        {
            PLYCON.StandCollisionExit();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "yuka")
        {
            PLYCON.StandCollisionHit();
        }
    }


}
