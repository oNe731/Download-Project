using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconButton : MonoBehaviour, IPointerClickHandler
{
    private FileIconSlot m_owner;
    private AudioSource m_audioSource;

    public void Set_Owner(FileIconSlot owner)
    {
        m_audioSource = GetComponent<AudioSource>();

        m_owner = owner;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_owner == null || GameManager.Ins.IsGame == false || m_owner.IsClickState == false)
            return;

        m_audioSource.Play();
        m_owner.OnPointerClick();
    }
}
