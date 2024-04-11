using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayLight : MonoBehaviour
{
    private Light light;
    private float time;
    private float changeTime;

    private void Start()
    {
        light = GetComponent<Light>();
        changeTime = Random.Range(1.0f, 3.0f);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > changeTime)
        {
            time = 0.0f;
            changeTime = Random.Range(0.5f, 2.0f);
            light.enabled = !light.enabled;
        }
    }
}
