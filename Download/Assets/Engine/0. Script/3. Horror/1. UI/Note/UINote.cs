using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Horror
{
    public class UINote : MonoBehaviour
    {
        private enum PAGE { PAGE_ITEM, PAGE_CLUE, PAGE_END }
        private enum BUTTON { BUTTON_ITEM_ON, BUTTON_ITEM_OFF, BUTTON_CLUE_ON, BUTTON_CLUE_OFF, BUTTON_END }

        [SerializeField] private Image[] m_button;
        [SerializeField] private Sprite[] m_buttonImg;
        [SerializeField] private Sprite[] m_pageImg;
        [SerializeField] private NoteItemInfo m_panelInfo;
        [SerializeField] private NoteItemMagnifyingGlass m_panelMagnifyingGlass;

        private Note m_note = null;

        //private PAGE m_currentPage = PAGE.PAGE_ITEM;
        private Image m_page;
        private RectTransform m_rectTransform;

        private Vector3 m_closePosition = new Vector3(-647f, 0f, 0f);
        private Vector3 m_opnePosition = new Vector3(0f, 0f, 0f);
        private float m_duration = 0.8f;
        private Coroutine m_coroutine = null;

        public NoteItemInfo InfoPanel => m_panelInfo;

        public void Initialize_UINote()
        {
            m_note = GetComponent<Note>();
            m_page = transform.GetChild(1).GetChild(0).GetComponent<Image>();
            m_rectTransform = transform.GetChild(1).GetComponent<RectTransform>();
            m_rectTransform.anchoredPosition = m_closePosition;

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }

        public void Button_Item()
        {
            //m_currentPage = PAGE.PAGE_ITEM;
            m_button[(int)PAGE.PAGE_ITEM].sprite = m_buttonImg[(int)BUTTON.BUTTON_ITEM_ON];
            m_button[(int)PAGE.PAGE_CLUE].sprite = m_buttonImg[(int)BUTTON.BUTTON_CLUE_OFF];
            m_page.sprite = m_pageImg[(int)PAGE.PAGE_ITEM];

            m_note.ItemPageItems.gameObject.SetActive(true);
            m_note.CluePageItems.gameObject.SetActive(false);

            ActiveFalse_PanelInfo();
        }

        public void Button_Clue()
        {
            //m_currentPage = PAGE.PAGE_CLUE;
            m_button[(int)PAGE.PAGE_ITEM].sprite = m_buttonImg[(int)BUTTON.BUTTON_ITEM_OFF];
            m_button[(int)PAGE.PAGE_CLUE].sprite = m_buttonImg[(int)BUTTON.BUTTON_CLUE_ON];
            m_page.sprite = m_pageImg[(int)PAGE.PAGE_CLUE];

            m_note.ItemPageItems.gameObject.SetActive(false);
            m_note.CluePageItems.gameObject.SetActive(true);

            ActiveFalse_PanelInfo();
        }

        public void Opne_Note(bool open)
        {
            ActiveFalse_PanelInfo();

            if (m_coroutine != null)
                StopCoroutine(m_coroutine);

            if (open == true)
                m_coroutine = StartCoroutine(Move_Note(m_rectTransform.anchoredPosition, m_opnePosition, m_duration, open));
            else
                m_coroutine = StartCoroutine(Move_Note(m_rectTransform.anchoredPosition, m_closePosition, m_duration, open));
        }

        IEnumerator Move_Note(Vector3 startPosition, Vector3 targetPosition, float duration, bool open)
        {
            GameManager.Ins.Sound.Play_ManagerAudioSource("Horror_OpenNote", false, 1f);
            if (open == true)
            {
                GameManager.Ins.Set_Pause(true);
                Button_Item();

                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            }

            float time = 0f;
            while (time < duration)
            {
                m_rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            if (open == false)
            {
                GameManager.Ins.Set_Pause(false);

                m_button[(int)PAGE.PAGE_ITEM].sprite = m_buttonImg[(int)BUTTON.BUTTON_ITEM_OFF];
                m_button[(int)PAGE.PAGE_CLUE].sprite = m_buttonImg[(int)BUTTON.BUTTON_CLUE_OFF];

                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            }

            m_rectTransform.anchoredPosition = targetPosition;
            yield break;
        }

        private void ActiveFalse_PanelInfo()
        {
            m_panelInfo.gameObject.SetActive(false);
            m_panelMagnifyingGlass.gameObject.SetActive(false);
            if (m_note.NoteSlot != null)
                m_note.NoteSlot.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }
    }
}

