using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacles : MonoBehaviour
{
    public void Start_Tentacles(float symptomTime, float idleTime)
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            Tentacle tentacle = transform.GetChild(i).GetComponent<Tentacle>();
            if (tentacle != null)
                tentacle.Start_Tentacle(symptomTime, idleTime);
        }
    }

    private void Update()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
