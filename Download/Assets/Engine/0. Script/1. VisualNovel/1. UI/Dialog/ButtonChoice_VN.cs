using UnityEngine;
using UnityEngine.EventSystems;

namespace VisualNovel
{
    public class ButtonChoice_VN : MonoBehaviour, IPointerEnterHandler
    {
        public int m_buttonIndex;
        public int ButtonIndex
        {
            get { return m_buttonIndex; }
            set { m_buttonIndex = value; }
        }

        public Dialog_VN m_ownerDialog;
        public Dialog_VN Ownerdialog
        {
            set { m_ownerDialog = value; }
        }

        // 커서와 충돌했을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            m_ownerDialog.Enter_Button(m_buttonIndex);
        }
    }
}

