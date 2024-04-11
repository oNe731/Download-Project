using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class VisualNovelManager : MonoBehaviour
{
    enum LEVELSTATE { LS_NOVEL, LS_SHOOT, LS_CHASE, LS_FINISH, LS_END };

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

    [Header("[ Basic ]")]
    [SerializeField] private LEVELSTATE m_LevelState;

    [Header("[ Likeability ]")]
    [SerializeField] private GameObject m_likeability;
    [SerializeField] private DialogHeart[] m_dialogHeart;

    [Header("[ CD ]")]
    [SerializeField] private GameObject m_CD;
    [SerializeField] private TMP_Text m_TextCDCount;
    [SerializeField] private float m_minDistance = 20.0f;
    [SerializeField] private float m_maxDistance = 200.0f;
    [SerializeField] private int m_MaxCount = 5;
    [SerializeField] private int m_CurrentCount = 0;

    private Transform m_playerTr;

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
        m_playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        m_npcHeart = new int[(int)OWNER_TYPE.OT_END];
        for(int i = 0; i < (int)OWNER_TYPE.OT_END; i++)
            m_npcHeart[i] = 5;

        Create_CD();
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

    private void Active_Popup()
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

    private void Create_CD() // 추후에 레벨상태에 따라 [도입, 진행, 탈출] 함수 분리해서 사용
    {
        List<Vector3> beforePosition = new List<Vector3>();
        beforePosition.Add(new Vector3(0f, 0f, 0f));

        for (int i = 0; i < m_MaxCount; i++)
        {
            Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
            Instantiate(m_CD, newPosition, Quaternion.identity);
            beforePosition.Add(newPosition);
        }
    }

    private Vector3 Get_RandomPositionOnNavMesh(List<Vector3> beforePos)
    {
        Vector3 position = new Vector3();
        bool select = false;

        int loopNum = 0;
        while (!select)
        {
            Vector3 randomPos = m_playerTr.position + Random.insideUnitSphere * m_maxDistance; // 원하는 범위 내의 랜덤 방향 벡터 생성
            randomPos.y = 0.0f;
            NavMeshHit hit;

            // SamplePosition((Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
            // areaMask 에 해당하는 NavMesh 중에서 maxDistance 반경 내에서 sourcePosition에 가장 가까운 위치를 찾아서 그 결과를 hit에 담음
            if (NavMesh.SamplePosition(randomPos, out hit, m_maxDistance, NavMesh.AllAreas)) // 위치 샘플링을 성공하면 참을 반환
            {
                bool distMin = false;
                foreach (Vector3 pos in beforePos)
                {
                    float distX = Mathf.Abs(hit.position.x - pos.x);
                    float distZ = Mathf.Abs(hit.position.z - pos.z);
                    if (distX <= m_minDistance || distZ <= m_minDistance)
                        distMin = true;
                }

                if (!distMin)
                {
                    position = hit.position;
                    select   = true;
                }
            }

            if (loopNum++ > 10000)
                throw new System.Exception("Infinite Loop");
        }

        return position;
    }

    public void Add_CD()
    {
        m_CurrentCount++;
        if (m_CurrentCount >= m_MaxCount)
        {
            // 추격 게임 종료
        }
        else
            m_TextCDCount.text = m_CurrentCount.ToString();
    }
}
