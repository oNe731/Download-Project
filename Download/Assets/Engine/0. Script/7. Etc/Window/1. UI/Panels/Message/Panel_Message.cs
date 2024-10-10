using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Panel_Message : Panel_Popup
{
    [SerializeField] private TMP_Text[] m_playerName;

    void Start()
    {
        for(int i = 0; i < m_playerName.Length; ++i)
            m_playerName[i].text = GameManager.Ins.PlayerName;
    }

    
    void Update()
    {
        
    }
}
