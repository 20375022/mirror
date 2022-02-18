using UnityEngine;
using System.Collections;
using UnityEngine;

public class SColorChanger : MonoBehaviour
{
    public Material sonarmaterial;           // 割り当てるマテリアル.

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
//            SonarColorChange();
        }
    }

    void SonarColorChange() {
        this.GetComponent<Renderer>().material = sonarmaterial;
    }
}
