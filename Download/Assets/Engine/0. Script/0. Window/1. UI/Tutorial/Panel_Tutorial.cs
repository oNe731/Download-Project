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
        m_detail[0] = "�ٿ�ε� �Ϸ����� ���� ���� ȯ���մϴ�!\n" +
            "�ٿ�ε� �Ϸ����� �������� ����� ������ �ڱ��ϴ� �ü����, �����а� ������ �Բ��� ���Դϴ�.\n\n" +
            "��½�̴� ���� ���̿� ���� �ð��� �̹����� ����, ���� ȯ���� ��ο� ���� ������ ���� ������ �����ô��� <color=#FF0000>�����μ� ���� ����</color>�� ����ų �� ������ ���� ��Ź�帳�ϴ�.\n\n" +
            "��, ��Ȥ�ϰ� ������ �ð��� �̹����� ����� <color=#FF0000>���¼��� �ִ� ���縦 ���</color>�ϰ� ������ ���� �� ����� ����� �ֽñ� �ٶ��ϴ�.";

        m_detail[1] = "���۵� �������� <color=#FF0000>��� ���ǻ��׿� ������ ������ ����</color>�մϴ�.\n" +
            "���� ��ü�� �̻��� ����ðų� ������ ������ ��Ŵٸ� ������ �ߴ��Ͻð�, ����̿��� �ݵ�� ������ ��û�ϼ���.\n\n" +
            "[�ݱ�]�� �����ø�, ���������� �ٿ�ε� �Ϸ������� ������ ���۵˴ϴ�.\n\n" +
            "{{PLAYER_NAME}} ��, ȯ���մϴ�.";

        Button_Next();
    }

    public void Button_Next()
    {
        if(m_currentCount < m_maxCount)
        {
            // ���� ������Ʈ
            m_dialogTxt.text = m_detail[m_currentCount];

            // ��ư ������Ʈ
            if(m_currentCount == m_maxCount - 1)
                m_buttonTxt.text = "�ݱ�";
            else
                m_buttonTxt.text = "����";
            m_currentCount++;
        }
        else
        {
            // ���� ����
            // �������� ȭ�� �� ������ ����

            // â ����
            GameManager.Ins.Resource.Destroy(gameObject);
        }
    }
}
