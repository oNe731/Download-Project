using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualNovelManager : MonoBehaviour
{
    private static VisualNovelManager m_instance = null;
    public static VisualNovelManager Instance
    {
        get //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 호출 가능
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    [Header("Likeability")]
    [SerializeField] private GameObject m_likeability;
    [SerializeField] private DialogHeart[] m_dialogHeart;

    private int[] m_npcHeart;
    public int[] NpcHeart
    {
        get { return m_npcHeart; }
        set { m_npcHeart = value; }
    }

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
        m_npcHeart = new int[(int)OWNER_TYPE.OT_END];
        for(int i = 0; i < (int)OWNER_TYPE.OT_END; i++)
            m_npcHeart[i] = 5;

        m_npcHeart[0] = 1;
        m_npcHeart[1] = 4;
        m_npcHeart[2] = 6;
    }

    private void Update()
    {
        Update_Input();
    }

    private void Update_Input()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();
    }

    public void Active_Popup()
    {
        // 호감도창 비/활성화
        m_likeability.SetActive(!m_likeability.activeSelf);
        if (true == m_likeability.activeSelf)
        {
            // 호감도창 NPC정보 업데이트
            for (int i = 0; i < m_dialogHeart.Length; i++)
                m_dialogHeart[i].Update_Heart();
        }
    }
}
