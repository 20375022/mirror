using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public class LightAction : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float speed;
    [SerializeField] float intensity;

    Light light;
    float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        light.intensity = intensity * curve.Evaluate(t * speed);
    }

}
