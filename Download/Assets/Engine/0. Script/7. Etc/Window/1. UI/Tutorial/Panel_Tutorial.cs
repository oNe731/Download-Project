using TMPro;
using UnityEngine;

public class Panel_Tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text m_dialogTxt;
    [SerializeField] private TMP_Text m_buttonTxt;

    private int m_maxCount = 2;
    private int m_currentCount = 0;
    private string[] m_detail;

    private void Start()
    {
        m_detail = new string[2];
        m_detail[0] = "다운로드 일루전에 오신 것을 환영합니다!\n" +
            "다운로드 일루전은 여러분의 상상을 마음껏 자극하는 운영체제로, 여러분과 언제나 함께할 것입니다.\n\n" +
            "번쩍이는 빛과 무늬와 같은 시각적 이미지가 많고, 전시 환경이 어두워 발작 증세나 간질 병력이 없으시더라도 <color=#FF0000>광과민성 간질 발작</color>을 일으킬 수 있으니 주의 부탁드립니다.\n\n" +
            "또, 잔혹하고 무서운 시각적 이미지나 사운드와 <color=#FF0000>폭력성이 있는 소재를 사용</color>하고 있으니 시작 전 충분히 고려해 주시길 바랍니다.";

        m_detail[1] = "시작된 순간부터 <color=#FF0000>모든 주의사항에 동의한 것으로 간주</color>합니다.\n" +
            "도중 신체에 이상이 생기시거나 불편한 감정이 드신다면 게임을 중단하시고, 도우미에게 반드시 도움을 요청하세요.\n\n" +
            "[닫기]를 누르시면, 본격적으로 다운로드 일루전과의 모험이 시작됩니다.\n\n" +
            "{{PLAYER_NAME}} 님, 환영합니다.";

        Button_Next();
    }

    public void Button_Next()
    {
        if(m_currentCount < m_maxCount)
        {
            // 내용 업데이트
            m_dialogTxt.text = m_detail[m_currentCount];

            // 버튼 업데이트
            if(m_currentCount == m_maxCount - 1)
                m_buttonTxt.text = "닫기";
            else
                m_buttonTxt.text = "다음";
            m_currentCount++;
        }
        else
        {
            // 게임 시작
            // 이전까지 화면 앱 눌리지 않음

            // 창 삭제
            GameManager.Ins.Resource.Destroy(gameObject);
        }
    }
}
