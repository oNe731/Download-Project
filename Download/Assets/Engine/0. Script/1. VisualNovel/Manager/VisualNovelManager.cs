using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VNNPC { NPC_SIA, NPC_SOYUL, NPC_JIU, NPC_END };

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

    public int[] npcHeart;
    public int[] NpcHeart
    {
        get { return npcHeart; }
        set { npcHeart = value; }
    }

    // TestCode
    [Header("Test")]
    [SerializeField] private string m_dialogDataPath;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    void Start()
    {
        npcHeart = new int[(int)VNNPC.NPC_END];
        for(int i = 0; i < (int)VNNPC.NPC_END; i++)
            npcHeart[i] = 5;
    }

    void Update()
    {
        Update_Input();
    }

    private void Update_Input()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();

        // TestCode
        if (Input.GetKeyDown(KeyCode.A))
            DialogManager.Instance.Create_Dialog(m_dialogDataPath);
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
