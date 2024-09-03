using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDelete : MonoBehaviour
{
    private void LateUpdate()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
