using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetpos;
    public float Cam_sensi ;

    public GameObject OptionUI;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += target.transform.position - targetpos;
        targetpos = target.transform.position;

        // カメラ操作 -- キーボード入力による回転処理(横)
        if (Input.GetKey("left"))
        {
            targetpos = target.transform.position;
            transform.RotateAround(targetpos, Vector3.up, (Cam_sensi * -1) * Time.deltaTime);
        }
        else
        {
            if (Input.GetKey("right"))
            {
                targetpos = target.transform.position;
                transform.RotateAround(targetpos, Vector3.up, Cam_sensi * Time.deltaTime);
            }
        }
    }
}
