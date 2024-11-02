using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameErrorPopups : MonoBehaviour
{
    public enum STATE { ST_WAITPOPUP, ST_ACTIVEPOPUP, ST_ACTIVEWAIT, ST_DELETE, ST_END }

    private STATE m_state = STATE.ST_WAITPOPUP;
    private float m_time = 0f;

    private List<GameObject> m_popups = new List<GameObject>();
    private int m_activeCount = 0;

    private float m_delay = 0.45f; // 처음 딜레이
    private float m_minDelay = 0.02f; // 최소 딜레이
    private float m_accelerationRate = 0.7f; // 가속도, 딜레이가 줄어드는 비율

    private void Start()
    {
        for(int i = 1; i < transform.childCount; ++i)
            m_popups.Add(transform.GetChild(i).gameObject);
    }

    private void Update()
    {
        switch (m_state)
        {
            case STATE.ST_WAITPOPUP: // 10초 대기 후 인터넷 창 비활성화
                m_time += Time.deltaTime;
                if (m_time >= 10f)
                {
                    m_time = 0f;

                    GameManager.Ins.Window.Internet.InputPopupButton = true;
                    GameManager.Ins.Window.Internet.Active_Popup(false);

                    m_state = STATE.ST_ACTIVEPOPUP;
                }
                break;

            case STATE.ST_ACTIVEPOPUP: // 시간에 따른 팝업창 개별 활성화
                if (m_activeCount < m_popups.Count)
                {
                    m_time += Time.deltaTime;
                    if (m_time >= m_delay)
                    {
                        m_time = 0f;
                        Create_Popup();

                        m_delay *= m_accelerationRate; // 창 활성화 시간 단축
                        if (m_delay < m_minDelay)
                            m_delay = m_minDelay;
                    }
                }
                else // 다 활성화 시 다음 상태로 변경
                {
                    m_time = 0f;
                    m_state = STATE.ST_ACTIVEWAIT;
                }
                break;

            case STATE.ST_ACTIVEWAIT: // 1초 대기 후 빨간 화면 활성화
                m_time += Time.deltaTime;
                if (m_time >= 1f)
                {
                    m_time = 0f;

                    transform.GetChild(0).gameObject.SetActive(true); // 빨간화면오브젝트
                    transform.GetChild(0).gameObject.transform.SetAsLastSibling();

                    m_state = STATE.ST_DELETE;
                }
                break;

            case STATE.ST_DELETE: // 1초 대기 후 에러 창 삭제 및 zip 파일 추가
                m_time += Time.deltaTime;
                if (m_time >= 1f)
                {
                    m_time = 0f;

                    // ZIP 파일 생성
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_ZIP, "Zip", () => GameManager.Ins.Window.Folder.Active_Popup(true, (int)Panel_Folder.TYPE.TYPE_GAMEZIP));
                    GameManager.Ins.Window.Set_WindowFileChildFile(GameManager.Ins.Window.BackgroundPath, "Zip", "4. Data/0. Window/Folders/Folders_Games");

                    Destroy(gameObject);
                    m_state = STATE.ST_END;
                }
                break;
        }
    }

    private void Create_Popup()
    {
        GameObject gameObject;
        do
        {
            int index = Random.Range(0, m_popups.Count);
            gameObject = m_popups[index];
        } while (gameObject.activeSelf);

        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-700f, 700f), Random.Range(-400f, 400f));
        gameObject.transform.SetAsLastSibling();
        gameObject.SetActive(true);

        m_activeCount++;
    }
}
