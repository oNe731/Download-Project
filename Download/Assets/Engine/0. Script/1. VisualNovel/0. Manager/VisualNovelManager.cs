using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;
using TMPro;

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

    // 미연시 -> 사격 -> 미연시 -> 추격 => 서부 레벨
    public enum LEVELSTATE { LS_NOVELBEGIN, LS_SHOOTGAME, LS_NOVELEND, LS_CHASEGAME, LS_END };
    public enum NPCTYPE { OT_WHITE, OT_BLUE, OT_YELLOW, OT_PINK, OT_END };

    [SerializeField] GameObject m_StartPanel;
    [SerializeField] private LEVELSTATE m_StartState = LEVELSTATE.LS_END;
    private LEVELSTATE m_LevelState = LEVELSTATE.LS_END;

#region LS_NOVEL
    [Header("[ LS_NOVEL ]")]
    [SerializeField] private GameObject m_dialog;
    [SerializeField] private GameObject m_likeability;
    [SerializeField] private NpcLike[] m_dialogHeart;

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
    private bool m_shootGameNext = false;
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
    private float m_maxTime = 60f;
    private float m_overTime = 0f;
    [SerializeField] private DOLLTYPE m_dollType = DOLLTYPE.DT_END;
    public DOLLTYPE DollType
    {
        get { return m_dollType; }
        set { m_dollType = value; }
    }
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

    [SerializeField] private GameObject m_playerObj;
    private GameObject m_yandereObj;
    private HallwayPlayer m_player;
    private HallwayYandere m_yandere;
    private Transform m_playerTr;
    private Transform m_yandereTr;
    public GameObject PlayerObj { get { return m_playerObj; } }
    public HallwayPlayer Player { get { return m_player; } }
    public Transform PlayerTr { get { return m_playerTr; } }
#endregion

#region Resource
    private Dictionary<string, Sprite> m_backgroundSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_standingSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_portraitSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_boxISpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_ellipseSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_arrawSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_choiceButtonSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> m_heartSpr = new Dictionary<string, Sprite>();
    private Dictionary<string, TMP_FontAsset> m_fontAst = new Dictionary<string, TMP_FontAsset>();

    public Dictionary<string, Sprite> BackgroundSpr { get { return m_backgroundSpr; }}
    public Dictionary<string, Sprite> StandingSpr { get { return m_standingSpr; }}
    public Dictionary<string, Sprite> PortraitSpr { get { return m_portraitSpr; }}
    public Dictionary<string, Sprite> BoxISpr { get { return m_boxISpr; }}
    public Dictionary<string, Sprite> EllipseSpr { get { return m_ellipseSpr; }}
    public Dictionary<string, Sprite> ArrawSpr { get { return m_arrawSpr; } }
    public Dictionary<string, Sprite> ChoiceButtonSpr { get { return m_choiceButtonSpr; }}
    public Dictionary<string, Sprite> HeartSpr { get { return m_heartSpr; } }
    public Dictionary<string, TMP_FontAsset> FontAst { get { return m_fontAst; } }

    #endregion

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        Load_Resource();
        Create_NpcHeart();
    }

    private void Start()
    {
        //Change_Level(m_StartState);
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
    private void Create_NpcHeart()
    {
        m_npcHeart = new int[(int)NPCTYPE.OT_END];
        for (int i = 0; i < (int)NPCTYPE.OT_END; i++)
            m_npcHeart[i] = 2;//0;
    }

    private void Start_NovelBegin()
    {
        m_dialog.SetActive(true);

        m_dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog1_SchoolWay.json"));
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_2D);
    }

    private void Update_NovelBegin()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();
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
        m_dialog.SetActive(false);
        m_chaseGame.SetActive(false);
        m_shootGame.SetActive(true);

        m_time = m_maxTime;
        m_countTxt.text = m_time.ToString();

        UIManager.Instance.Start_FadeIn(1f, Color.black);
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_2D);
    }

    public void Play_ShootGame()
    {
        m_shootGameStart = true;
        CursorManager.Instance.Change_Cursor(CURSORTYPE.CT_NOVELSHOOT);
        m_belt.Start_Belt();
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
        Destroy(m_shootGame); // 재시작하지 않을 시 삭제
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

            m_dollType = DOLLTYPE.DT_FAIL;

        }
        else
        {
            int Count = (int)m_time;
            m_countTxt.text = Count.ToString();
        }
    }

    private void GameOver_ShootGame()
    {
        if (m_shootGameNext)
            return;

        // 인형 또는 쓰레기 1개 이상 획득해도 사격 게임 종료/ 미연시 시작 : 맞춘 종류에 따라 다음 대사가 다름.
        m_overTime += Time.deltaTime;
        if (m_overTime > 1.5f)
        {
            if(!m_belt.OverEffect)
                m_belt.Over_Game(); // 2) 1.5초 뒤 인형 전부 폭발
            else if(!m_shootGameNext && m_overTime > 3) // 3) 1.5초 뒤 페이드 아웃으로 전환
            {
                m_shootGameNext = true;
                UIManager.Instance.Start_FadeOut(1f, Color.black, () => Change_Level(LEVELSTATE.LS_NOVELEND), 0.5f, false);
            }
        }
    }
#endregion

#region LS_NOVELEND
    private void Start_NovelEnd()
    {
        switch (m_dollType)
        {
            case DOLLTYPE.DT_BIRD:
                m_dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollBird.json"));
                break;

            case DOLLTYPE.DT_SHEEP:
                m_dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollSheep.json"));
                break;

            case DOLLTYPE.DT_CAT:
                m_dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollCat.json"));
                break;

            case DOLLTYPE.DT_FAIL:
                m_dialog.GetComponent<Dialog_VN>().Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog2_DollFail.json"));
                break;
        }

        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_2D);
    }

    private void Update_NovelEnd()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Active_Popup();
    }

    private void Finish_NovelEnd()
    {

    }
#endregion

#region LS_CHASEGAME
    private void Start_ChaseGame()
    {
        m_player   = m_playerObj.GetComponent<HallwayPlayer>();
        m_playerTr = m_playerObj.GetComponent<Transform>();

        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_3D);
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
        CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
        camera.Change_Position(new Vector3(0f, 1.4f, -2.8f));
        camera.Change_Rotation(new Vector3(8.5f, 0f, 0f));

        // 지하실 다이얼로그 시작 (페이드 인)
        Dialog_VN dialog = m_dialog.GetComponent<Dialog_VN>();
        dialog.Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog5_Cellar.json"));
        dialog.Close_Background();

        //// m_shootGame.SetActive(false);
        m_chaseGame.SetActive(true);
    }

    public void Play_ChaseGame()
    {
        m_dialog.SetActive(false);

        Create_CD();
        Create_Lever(m_LeverMaxCount);
        m_player.Set_Lock(false);

        UIManager.Instance.Start_FadeIn(1f, Color.black);
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

    public void Create_Monster()
    {
        // 캐릭터 락
        m_player.Set_Lock(true);

        // 카메라 교체 및 설정
        CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
        CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
        camera.Change_Position(new Vector3(0f, 1.2f, 20f));
        camera.Change_Rotation(new Vector3(0f, -180f, 25f));

        // 얀데레 생성
        m_yandereObj = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Character/Yandere"));
        m_yandere = m_yandereObj.GetComponent<HallwayYandere>();
        m_yandereTr = m_yandereObj.GetComponent<Transform>();
        m_yandereTr.position = new Vector3(0f, 0f, 2.8f);

        // 페이드 인
        UIManager.Instance.Start_FadeIn(1f, Color.black);
        // 돌면서 특정거리까지 줌인
        camera.Start_Cutscene(new Vector3(0f, 1.2f, 5.5f), new Vector3(0f, 180f, -16f), 2f, 0.5f);  
        // 캐릭터가 말을 할때 얀데레 얼굴 클로즈업
        // 
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
        if(m_yandereObj != null)
            m_yandere.Used_Lever();

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

    public void Button_Start()
    {
        Destroy(m_StartPanel);
        Change_Level(m_StartState);
    }

    public void Button_Exit()
    {
        SceneManager.LoadScene("Window");
    }

    private void Load_Resource()
    {
        // 배경 이미지 할당
        m_backgroundSpr.Add("BackGround_SchoolWay",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_SchoolWay"));
        m_backgroundSpr.Add("BackGround_School",      Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_School"));
        m_backgroundSpr.Add("BackGround_NightMarket", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_NightMarket"));
        m_backgroundSpr.Add("BackGround_Festival",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Festival"));
        m_backgroundSpr.Add("BackGround_PlayerHome",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PlayerHome"));
        m_backgroundSpr.Add("BackGround_PinkHome",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_PinkHome"));
        m_backgroundSpr.Add("BackGround_Cellar",      Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/BackGround/BackGround_Cellar"));
        
        // 스탠딩 이미지 할당
        m_standingSpr.Add("Blue",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Standing/Blue"));
        m_standingSpr.Add("Pink",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Standing/Pink"));
        m_standingSpr.Add("Yellow", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Standing/Yellow"));

        // 프로필 이미지 할당
        m_portraitSpr.Add("Blue",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/Blue"));
        m_portraitSpr.Add("Pink",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/Pink"));
        m_portraitSpr.Add("Yellow", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/Yellow"));
        m_portraitSpr.Add("White",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Portrait/White"));

        // 박스 이미지 할당
        m_boxISpr.Add("UI_VisualNovel_Blue_ChatBox",        Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Blue_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Pink_ChatBox",        Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Pink_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_White_ChatBox",       Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_White_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Yellow_ChatBox",      Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/ChatBox/UI_VisualNovel_Yellow_ChatBox"));
        m_boxISpr.Add("UI_VisualNovel_Blue_NarrationBox",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Blue_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_Pink_NarrationBox",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Pink_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_White_NarrationBox",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_White_NarrationBox"));
        m_boxISpr.Add("UI_VisualNovel_Yellow_NarrationBox", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Box/NarrationBox/UI_VisualNovel_Yellow_NarrationBox"));

        // 원 아이콘 이미지 할당
        m_ellipseSpr.Add("UI_VisualNovel_Blue_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Blue_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_Pink_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Pink_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_White_Ellipse",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_White_Ellipse"));
        m_ellipseSpr.Add("UI_VisualNovel_Yellow_Ellipse", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Yellow_Ellipse"));

        // 넘김표시 이미지 할당
        m_arrawSpr.Add("UI_VisualNovel_Blue_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Blue_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_Pink_Ellipse",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Pink_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_White_Ellipse",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_White_Ellipse"));
        m_arrawSpr.Add("UI_VisualNovel_Yellow_Ellipse", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Ellipse/UI_VisualNovel_Yellow_Ellipse"));

        // 버튼 이미지 할당
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonOFF", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_White_ButtonOFF"));
        m_choiceButtonSpr.Add("UI_VisualNovel_White_ButtonON",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Button/UI_VisualNovel_White_ButtonON"));

        // 하트 이미지 할당
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartOFF",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Blue_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Blue_FriendshipHeartON",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Blue_FriendshipHeartON"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartOFF",   Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Pink_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Pink_FriendshipHeartON",    Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Pink_FriendshipHeartON"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartOFF", Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Yellow_FriendshipHeartOFF"));
        m_heartSpr.Add("UI_VisualNovel_Yellow_FriendshipHeartON",  Resources.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/ChatScript/Heart/UI_VisualNovel_Yellow_FriendshipHeartON"));

        // 폰트 에셋 할당
        m_fontAst.Add("VN_Basic_Blue",    Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Blue/VN_Basic_Blue"));
        m_fontAst.Add("VN_Basic_RBlue",   Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Blue/VN_Basic_RBlue"));
        m_fontAst.Add("VN_Basic_Pink",    Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Pink/VN_Basic_Pink"));
        m_fontAst.Add("VN_Basic_RPink",   Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Pink/VN_Basic_RPink"));
        m_fontAst.Add("VN_Basic_White",   Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/White/VN_Basic_White"));
        m_fontAst.Add("VN_Basic_RWhite",  Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/White/VN_Basic_RWhite"));
        m_fontAst.Add("VN_Basic_Yellow",  Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Yellow/VN_Basic_Yellow"));
        m_fontAst.Add("VN_Basic_RYellow", Resources.Load<TMP_FontAsset>("3. Font/FontAsset/1. VisualNovel/Yellow/VN_Basic_RYellow"));
    }
}