/*  -----------------------------------------------------   */
/*         消えた時に何かのオブジェクトを道連れにする       */
/*  -----------------------------------------------------   */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDestroy : MonoBehaviour
{
    public GameObject followDestroyObject;


    void OnDestroy()
    {
        Destroy(followDestroyObject);
    }
}
