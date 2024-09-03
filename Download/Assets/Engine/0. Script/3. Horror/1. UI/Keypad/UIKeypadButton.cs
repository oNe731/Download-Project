using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIKeypadButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image    m_img;
    [SerializeField] private Sprite[] m_sprite;

    public void OnDisable()
    {
        m_img.sprite = m_sprite[0];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_img.sprite = m_sprite[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_img.sprite = m_sprite[0];
    }
}
