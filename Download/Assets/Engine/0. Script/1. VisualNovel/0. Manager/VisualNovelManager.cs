using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class VisualNovelManager : MonoBehaviour
{
    enum LEVELSTATE { LS_NOVEL, LS_SHOOT, LS_CHASE, LS_END };

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
    [SerializeField] private TMP_Text m_CdTextCount;
    [SerializeField] private int m_CdMaxCount = 5;
    [SerializeField] private int m_CdCurrentCount = 0;
    [SerializeField] private float m_CdMinDistance = 20.0f;
    [SerializeField] private float m_CdMaxDistance = 200.0f;

    [Header("[ Item ]")]
    [SerializeField] private GameObject m_Lever;
    [SerializeField] private int m_LeverMaxCount = 2;
    [SerializeField] private Transform[] m_RandomPos;
    private List<GameObject> m_Levers = new List<GameObject>();

    private Transform m_playerTr;
    private GameObject m_boss;

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
        m_boss = GameObject.FindWithTag("Boss");

        m_npcHeart = new int[(int)OWNER_TYPE.OT_END];
        for(int i = 0; i < (int)OWNER_TYPE.OT_END; i++)
            m_npcHeart[i] = 5;

        Start_ChaseGame();
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

    // # Start_Chase ---------------------------------------------------------------------------
    private void Start_ChaseGame() // 추후에 레벨상태에 따라 [도입, 진행, 탈출] 함수 분리해서 사용
    {
        Create_CD();
        Create_Lever(m_LeverMaxCount);


    }

    private void Create_CD()
    {
        List<Vector3> beforePosition = new List<Vector3>();
        beforePosition.Add(new Vector3(0f, 0f, 0f));

        for (int i = 0; i < m_CdMaxCount; i++)
        {
            Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
            Instantiate(m_CD, newPosition, Quaternion.identity);
            beforePosition.Add(newPosition);
        }
    }

    public void Add_CD()
    {
        m_CdCurrentCount++;
        if (m_CdCurrentCount >= m_CdMaxCount)
        {
            // 추격 게임 종료
        }
        else
        {
            // 대사 출력

            // UI 업데이트
            m_CdTextCount.text = m_CdCurrentCount.ToString();
        }

    }

    private void Create_Lever(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            Vector3 NewPosition = Vector3.zero;
            while(true)
            {
                NewPosition = m_RandomPos[Random.Range(0, 20)].position;

                bool Same = false;
                for (int j = 0; j < m_Levers.Count; j++)
                {
                    if (NewPosition == m_Levers[j].transform.position)
                        Same = true;
                }

                if (!Same)
                    break;
            }

            GameObject level = Instantiate(m_Lever, NewPosition, Quaternion.identity);
            m_Levers.Add(level);
        }
    }

    public void Use_Lever(GameObject self)
    {
        // 아이템 효과 적용
        m_boss.GetComponent<HallwayYandere>().Used_Lever();

        // 현재 아이템 삭제
        for (int i = 0; i < m_Levers.Count; i++)
        {
            if (self == m_Levers[i])
            {
                m_Levers.RemoveAt(i);
                Destroy(self);
                break;
            }
        }

        // 추가 생성
        Create_Lever(1);
    }

    private Vector3 Get_RandomPositionOnNavMesh(List<Vector3> beforePos)
    {
        Vector3 position = new Vector3();
        bool select = false;

        int loopNum = 0;
        while (!select)
        {
            Vector3 randomPos = m_playerTr.position + Random.insideUnitSphere * m_CdMaxDistance; // 원하는 범위 내의 랜덤 방향 벡터 생성
            randomPos.y = 0.0f;
            NavMeshHit hit;

            // SamplePosition((Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
            // areaMask 에 해당하는 NavMesh 중에서 maxDistance 반경 내에서 sourcePosition에 가장 가까운 위치를 찾아서 그 결과를 hit에 담음
            if (NavMesh.SamplePosition(randomPos, out hit, m_CdMaxDistance, NavMesh.AllAreas)) // 위치 샘플링을 성공하면 참을 반환
            {
                bool distMin = false;
                foreach (Vector3 pos in beforePos)
                {
                    float distX = Mathf.Abs(hit.position.x - pos.x);
                    float distZ = Mathf.Abs(hit.position.z - pos.z);
                    if (distX <= m_CdMinDistance || distZ <= m_CdMinDistance)
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
}
