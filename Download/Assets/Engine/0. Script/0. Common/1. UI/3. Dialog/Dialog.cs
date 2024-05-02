using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog<T> : MonoBehaviour
{
    [SerializeField] protected T[] m_dialogs;

    protected bool  m_isTyping     = false;
    protected bool  m_cancelTyping = false;
    protected int   m_dialogIndex  = 0;
    protected float m_typeSpeed    = 0.05f;
    protected float m_arrowSpeed   = 0.5f;
}
