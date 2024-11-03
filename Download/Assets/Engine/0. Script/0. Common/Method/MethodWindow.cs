using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodWindow : MonoBehaviour
{
    private float m_time = 0f;
    private Action m_Delete = null;
    public Action DeleteAction { set => m_Delete = value; }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time < 1.5f)
            return;

        if (GameManager.Ins.Get_AnyKeyDown())
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_Delete == null) return;
        m_Delete?.Invoke();
    }
}
