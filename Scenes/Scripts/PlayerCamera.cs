using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetpos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // カメラ操作 -- キーボード入力による回転処理(横)
        var view_sensitivity = 1.5f;
        if (Input.GetKey("left"))
        {
            targetpos = target.transform.position;
            transform.RotateAround(targetpos, Vector3.up, -50.0f * Time.deltaTime);
        }
        else
        {
            if (Input.GetKey("right"))
            {
                view_sensitivity *= -1;
                targetpos = target.transform.position;
                transform.RotateAround(targetpos, Vector3.up, 50.0f * Time.deltaTime);
            }
        }        
    }
}
