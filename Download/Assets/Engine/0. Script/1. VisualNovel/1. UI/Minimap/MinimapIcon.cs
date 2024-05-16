using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class MinimapIcon : MonoBehaviour
    {
        private Transform m_playerTr;

        private void Start()
        {
            m_playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(90.0f, m_playerTr.eulerAngles.y, 0.0f);
        }
    }
}

