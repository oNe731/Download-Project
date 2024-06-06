using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VisualNovel;

public enum TRIGERTYPE { TG_END };

public class Trigerbox : MonoBehaviour
{
    [SerializeField] private TRIGERTYPE m_type = TRIGERTYPE.TG_END;

    private void OnTriggerEnter(Collider other)
    {
        switch(m_type)
        {
        }
    }
}
