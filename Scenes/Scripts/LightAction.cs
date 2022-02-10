using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LightAction : NetworkBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float speed;
    [SerializeField] float intensity;

    Light light;
    [SyncVar]
    float t = 0f;
    float tim;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        timeInc();
        timeSync();
        light.intensity = intensity * curve.Evaluate(tim * speed);
    }

    [ServerCallback]
    void timeInc()
    {
        t += Time.deltaTime;
        tim = t;
    }

    [ClientCallback]
    void timeSync()
    {
        tim = GetComponent<LightAction>().t;
    }

}
