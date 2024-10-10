using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MessageList : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject m_chat;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_chat.SetActive(true);
    }
}
