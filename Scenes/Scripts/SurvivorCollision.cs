using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorCollision : MonoBehaviour
{
    public PlayerControl PLYCON;
    public BoxCollider bCol;
    // 衝突しているオブジェクトリスト
    private List<GameObject> m_hitObjects = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        if (PLYCON.Killerflg == true)
        {
            bCol.enabled = false;
        }else
        {
            bCol.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (m_hitObjects.Count >= 2)
        {
            // TODO:2つ以上同時に衝突していた場合の処理
            PLYCON.SurvivorCollisionHit();
        }

        // 毎回衝突したオブジェクトリストをクリアする
        m_hitObjects.Clear();
    }

/*    void OnTriggerStay(Collider i_other)
    {
        // 衝突しているオブジェクトをリストに登録する
        m_hitObjects.Add(i_other.gameObject);
    }
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("プレイヤー発見");
            m_hitObjects.Add(other.gameObject);
        }
    }
}
