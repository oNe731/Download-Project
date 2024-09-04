using UnityEngine;
using UnityEngine.UI;

public class NoteItemMagnifyingGlass : MonoBehaviour
{
    [SerializeField] private Image m_image;
    private float m_lastClickTime = 0f;
    private float m_doubleClickThreshold = 1f;

    private void Update()
    {
        // 화면 두번 클릭하면 비활성화
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - m_lastClickTime; // 현재 시간과 마지막 클릭 시간 차이 계산
            if (timeSinceLastClick <= m_doubleClickThreshold)       // 두 번 클릭
                gameObject.SetActive(false);
            
            m_lastClickTime = Time.time; // 마지막 클릭 시간 업데이트
        }
    }

    public void Update_UIInfo(NoteItem item)
    {
        m_image.sprite = HorrorManager.Instance.NoteElementIcon[item.m_imageName + "_3"];
    }
}
