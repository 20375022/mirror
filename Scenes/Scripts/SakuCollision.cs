using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SakuCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ゲームオブジェクト同士が接触したタイミングで実行
    void OnTriggerEnter(Collider other)
    {
        // もし衝突した相手オブジェクトが柵ならば
        if (other.tag == "Player")
        {
            Debug.Log("柵に当たった");
            if (Input.GetKey(KeyCode.Space))
            {
            }
        }
    }

    // ゲームオブジェクト同士が接触している間実行
    void OnTriggerStay(Collider other)
    {
        // もし衝突した相手オブジェクトが柵ならば
        if (other.tag == "Player")
        {
            Debug.Log("柵に当たっている");
            if (Input.GetKey(KeyCode.Space))
            {
            }
        }
    }

    // ゲームオブジェクト同士が離れたタイミングで実行
    void OnTriggerExit(Collider other)
    {
        // もし衝突した相手オブジェクトが柵ならば
        if (other.tag == "Player")
        {
            Debug.Log("柵から離れた");
            if (Input.GetKey(KeyCode.Space))
            {
            }
        }
    }

}
