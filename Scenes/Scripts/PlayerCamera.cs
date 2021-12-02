using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetpos;
    public float hori_sensitivity = 50.0f;   // マウス感度

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
        // マウスの移動量を変数に封じ込める
        float rotX = Input.GetAxis("Mouse X");

        transform.RotateAround(targetpos, Vector3.up, hori_sensitivity * rotX);
    }
}
