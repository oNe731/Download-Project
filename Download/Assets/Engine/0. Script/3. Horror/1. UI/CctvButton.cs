using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CctvButton : MonoBehaviour
{
    [SerializeField] private int m_index = -1;

    public void Click_Button()
    {
        HorrorManager.Instance.Change_CCTV(m_index);
    }
}
