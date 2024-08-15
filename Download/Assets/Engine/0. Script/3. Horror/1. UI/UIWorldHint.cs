using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UIWorldHint : MonoBehaviour
{
    public enum HINTTYPE { HT_RESEARCH, HT_OPENDOOR, HT_END };

    [SerializeField] private TMP_Text m_text;

    [SerializeField] private Transform m_target = null;
    [SerializeField] private Vector3 m_offset = Vector3.zero;

    public void Initialize_UI(HINTTYPE hinttype, Transform transform, Vector3 m_uiOffset)
    {
        switch (hinttype)
        {
            case HINTTYPE.HT_RESEARCH:
                m_text.text = "조사하기";
                break;

            case HINTTYPE.HT_OPENDOOR:
                m_text.text = "문열기";
                break;
        }

        m_target = transform;
        m_offset = m_uiOffset;

        Update_Transform();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_target == null)
            return;

        Update_Transform();
    }

    public void Update_Transform()
    {
        // 위치
        transform.position = m_target.position + m_offset;

        // 회전
        Quaternion newRotation = Camera.main.transform.rotation * Quaternion.Euler(0, 180, 0);
        transform.LookAt(transform.position + newRotation * Vector3.forward, newRotation * Vector3.up);
    }
}
