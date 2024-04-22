using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

// # 미연시 -> 사격 -> 미연시 -> 추격 ==> 서부(다음 씬)
public enum LEVELSTATE { LS_NOVELBEGIN, LS_SHOOTGAME, LS_NOVELEND, LS_CHASEGAME, LS_END };

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

    [SerializeField] private LEVELSTATE m_StartState = LEVELSTATE.LS_END;
    private LEVELSTATE m_LevelState = LEVELSTATE.LS_END;

#region LS_NOVEL
    [Header("[ LS_NOVEL ]")]
    [SerializeField] private GameObject m_likeability;
    [SerializeField] private DialogHeart[] m_dialogHeart;

    private int[] m_npcHeart;
    public int[] NpcHeart
    {
        get { return m_npcHeart; }
        set { m_npcHeart = value; }
    }
#endregion

#region LS_SHOOT
    [Header("[ LS_SHOOT ]")]
    [SerializeField] private GameObject m_shootGame;
    [SerializeField] private TMP_Text m_countTxt;
    [SerializeField] private ShootContainerBelt m_belt;
    private bool m_shootGameStart = false;
    private bool m_shootGameOver = false;
    private bool m_shootGameStop = false;
    public bool ShootGameStart
    {
        get { return m_shootGameStart; }
        set { m_shootGameStart = value; }
    }
    public bool ShootGameOver
    {
        get { return m_shootGameOver; }
        set { m_shootGameOver = value; }
    }
    public bool ShootGameStop
    {
        get { return m_shootGameStop; }
        set { m_shootGameStop = value; }
    }
    private float m_time    = 0f;
    private float m_maxTime = 30f;
    private float m_overTime = 0f;
#endregion

#region LS_CHASE
    [Header("[ LS_CHASE ]")]
    [SerializeField] private GameObject m_chaseGame;
    [SerializeField] private GameObject m_Cd;
    [SerializeField] private TMP_Text m_CdTextCount;
    [SerializeField] private int m_CdMaxCount = 5;
    [SerializeField] private int m_CdCurrentCount = 0;
    [SerializeField] private float m_CdMinDistance = 20.0f;
    [SerializeField] private float m_CdMaxDistance = 200.0f;

    [SerializeField] private GameObject m_Lever;
    [SerializeField] private int m_LeverMaxCount = 2;
    [SerializeField] private Transform[] m_RandomPos;

    [SerializeField] private List<HallwayLight> m_Light; // 464
    public List<HallwayLight> Light
    {
        get { return m_Light; }
        set { m_Light = value; }
    }

    private List<GameObject> m_Levers = new List<GameObject>();
    private Transform m_playerTr;
    private GameObject m_boss;
#endregion

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
        Change_Level(m_StartState);
    }

    private void Update()
    {
        Update_Level(m_LevelState);
    }

#region Basic
    public void Change_Level(LEVELSTATE level)
    {
        if(m_LevelState != LEVELSTATE.LS_END)
            Finish_Level(m_LevelState);

        m_LevelState = level;

        Start_Level(m_LevelState);
    }

    private void Start_Level(LEVELSTATE level)
    {
        switch (level)
        {
            case LEVELSTATE.LS_NOVELBEGIN:
                Start_NovelBegin();
                break;

            case LEVELSTATE.LS_SHOOTGAME:
                Start_ShootGame();
                break;

            case LEVELSTATE.LS_NOVELEND:
                Start_NovelEnd();
                break;

            case LEVELSTATE.LS_CHASEGAME:
                Start_ChaseGame();
                break;
        }
    }

    private void Update_Level(LEVELSTATE level)
    {
        switch (level)
        {
            case LEVELSTATE.LS_NOVELBEGIN:
                Update_NovelBegin();
                break;

            case LEVELSTATE.LS_SHOOTGAME:
                Update_ShootGame();
                break;

            case LEVELSTATE.LS_NOVELEND:
                Update_NovelEnd();
                break;

            case LEVELSTATE.LS_CHASEGAME:
                Update_ChaseGame();
                break;
        }
    }

    private void Finish_Level(LEVELSTATE level)
    {
        switch (level)
        {
            case LEVELSTATE.LS_NOVELBEGIN:
                Finish_NovelBegin();
                break;

            case LEVELSTATE.LS_SHOOTGAME:
                Finish_ShootGame();
                break;

            case LEVELSTATE.LS_NOVELEND:
                Finish_NovelEnd();
                break;

            case LEVELSTATE.LS_CHASEGAME:
                Finish_ChaseGame();
                break;
        }
    }
#endregion

#region LS_NOVELBEGIN
    private void Start_NovelBegin()
    {
        m_npcHeart = new int[(int)OWNER_TYPE.OT_END];
        for (int i = 0; i < (int)OWNER_TYPE.OT_END; i++)
            m_npcHeart[i] = 7;
    }

    private void Update_NovelBegin()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();

        // 사격 게임 해보자 다이얼로그 나온 후에 클릭 등 입력 시 미연시 종료/ 사격 게임 시작
        // Change_Level(LEVELSTATE.LS_SHOOTGAME);
    }

    private void Finish_NovelBegin()
    {
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
#endregion

#region LS_SHOOTGAME
    private void Start_ShootGame()
    {
        m_chaseGame.SetActive(false);
        m_shootGame.SetActive(true);

        m_time = m_maxTime;
    }

    private void Update_ShootGame()
    {
        if (!m_shootGameStart || m_shootGameStop)
            return;

        if (!m_shootGameOver)
            Update_Count();
        else
            GameOver_ShootGame();
    }

    private void Finish_ShootGame()
    {
        CursorManager.Instance.Change_Cursor(CURSORTYPE.CT_ORIGIN);
        Destroy(m_shootGame);
    }

    private void Update_Count()
    {
        m_time -= Time.deltaTime;
        if(m_time <= 0.5f)
        {
            int Count = 0;
            m_countTxt.text = Count.ToString();

            m_shootGameOver = true;
            m_belt.UseBelt = false; // 1) 인형 일시 정지

        }
        else
        {
            int Count = (int)m_time;
            m_countTxt.text = Count.ToString();
        }
    }

    private void GameOver_ShootGame()
    {
        // 인형 또는 쓰레기 1개 이상 획득해도 사격 게임 종료/ 미연시 시작 : 맞춘 종류에 따라 다음 대사가 다름.
        m_overTime += Time.deltaTime;
        if (m_overTime > 1.5f)
        {
            if(!m_belt.OverEffect)
                m_belt.Over_Game(); // 2) 1.5초 뒤 인형 전부 폭발
            else if(m_overTime > 3) // 3) 1.5초 뒤 페이드 아웃으로 전환
                Change_Level(LEVELSTATE.LS_NOVELEND);
        }
    }
#endregion

#region LS_NOVELEND
    private void Start_NovelEnd()
    {

    }

    private void Update_NovelEnd()
    {
        // 어 잠이 온다 다이얼로그 나온 후에 암전, 클릭 등 입력 시 미연시 종료/ 추격 게임 시작
        // Change_Level(LEVELSTATE.LS_CHASEGAME); 
    }

    private void Finish_NovelEnd()
    {

    }
#endregion

#region LS_CHASEGAME
    private void Start_ChaseGame()
    {
        m_shootGame.SetActive(false);
        m_chaseGame.SetActive(true);

        m_playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        m_boss = GameObject.FindWithTag("Boss");

        Create_CD();
        Create_Lever(m_LeverMaxCount);

        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_FOLLOW);
    }

    private void Update_ChaseGame()
    {
    }

    private void Finish_ChaseGame()
    {
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_END);
    }

    private void Clear_ChaseGame()
    {
        // 게임 클리어 : CD 5개 다 모을 시 컷씬 진행 후 전환(다음 씬 서부로 전환)
    }

    private void Fail_ChaseGame()
    {
        // 게임 실패 : 얀데레한테 잡힐 시 컷씬 진행 후 복도 시작부터 다시 시작(재도전 UI 출력)
    }

    private void Create_CD()
    {
        List<Vector3> beforePosition = new List<Vector3>();
        beforePosition.Add(new Vector3(0f, 0f, 0f));

        for (int i = 0; i < m_CdMaxCount; i++)
        {
            Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
            Instantiate(m_Cd, newPosition, Quaternion.identity);
            beforePosition.Add(newPosition);
        }
    }

    private void Create_Lever(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Vector3 NewPosition = Vector3.zero;
            while (true)
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

    public void Get_CD()
    {
        m_CdCurrentCount++;
        if (m_CdCurrentCount >= m_CdMaxCount)
        {
            // 추격 게임 종료
            Finish_ChaseGame();
        }
        else
        {
            // UI 업데이트
            m_CdTextCount.text = m_CdCurrentCount.ToString();

            // 조명 업데이트 Max 464
            m_Light.Shuffle();
            int OnCount = (int)(464 / (m_CdMaxCount - 1)) * m_CdCurrentCount;
            for (int i = 0; i < OnCount; ++i)
                m_Light[i].Blink = true;

            // 대사 출력
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
                    select = true;
                }
            }

            if (loopNum++ > 10000)
                throw new System.Exception("Infinite Loop");
        }

        return position;
    }
#endregion
}