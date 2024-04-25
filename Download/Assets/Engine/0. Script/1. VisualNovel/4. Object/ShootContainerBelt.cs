using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootContainerBelt : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject[] m_Dolls;

    [Header("Resource")]
    [SerializeField] private GameObject m_Hpbar;

    private bool m_useBelt = false;
    public bool UseBelt
    {
        get { return m_useBelt; }
        set { m_useBelt = value; }
    }

    private float m_speed = 6f;

    private float m_UpLineDir   = -1f;
    private float m_DownLineDir = 1f;
    private float m_Time = 0f;
    private float m_Chang = 0f;
    private float m_ChangMin = 6f;
    private float m_ChangMax = 7f;

    private bool m_TurnEvent = false;
    private float m_EventTime = 0f;
    private float m_EventChang = 0f;
    private float m_EventChangMin = 2f;
    private float m_EventChangMax = 4f;
    private float m_TurnTime = 0f;
    private float m_TurnDuration = 0.2f;

    private bool m_overEffect = false;
    public bool OverEffect
    {
        get { return m_overEffect; }
    }

    private ShootDoll[] m_doll;

    private void Start()
    {
        m_doll = new ShootDoll[m_Dolls.Length];
        for (int i = 0; i < m_Dolls.Length; ++i)
        {
            m_doll[i] = m_Dolls[i].GetComponent<ShootDoll>();
            m_doll[i].Belt = this;
        }

        m_Chang = Random.Range(m_ChangMin, m_ChangMax);
        m_EventChang = Random.Range(m_EventChangMin, m_EventChangMax);
    }

    private void Update()
    {
        if (!m_useBelt)
            return;

        // 움직임 이벤트 발생
        m_EventTime += Time.deltaTime;
        if (m_EventTime >= m_EventChang)
        {
            m_EventTime = 0f;
            m_EventChang = Random.Range(m_EventChangMin, m_EventChangMax);

            m_TurnEvent = true;
        }

        if (m_TurnEvent)
        {
            for (int i = 0; i < m_doll.Length; ++i)
            {
                if (m_doll[i].Line == 1)
                    m_Dolls[i].transform.Translate((m_DownLineDir * -1 * m_speed) * Time.deltaTime, 0, 0);
                else if (m_doll[i].Line == 2)
                    m_Dolls[i].transform.Translate((m_UpLineDir * -1 * m_speed) * Time.deltaTime, 0, 0);
            }

            m_TurnTime += Time.deltaTime;
            if (m_TurnTime >= m_TurnDuration)
            {
                m_TurnTime = 0f;
                m_TurnEvent = false;
            }
        }
        else
        {
            // 움직임 전환 발생
            m_Time += Time.deltaTime;
            if (m_Time >= m_Chang)
            {
                m_Time = 0f;
                m_Chang = Random.Range(m_ChangMin, m_ChangMax);

                m_DownLineDir *= -1;
                m_UpLineDir *= -1;
            }

            for (int i = 0; i < m_doll.Length; ++i)
            {
                if (m_doll[i].Line == 1)
                    m_Dolls[i].transform.Translate((m_DownLineDir * m_speed) * Time.deltaTime, 0, 0);
                else if (m_doll[i].Line == 2)
                    m_Dolls[i].transform.Translate((m_UpLineDir * m_speed) * Time.deltaTime, 0, 0);
            }
        }
    }

    private void LateUpdate()
    {
        if (!m_useBelt)
            return;

        // 커튼 속으로 들어가면 다른 라인 시작 위치로 이동해서 해당 라인 이동 방향으로 이동
        for (int i = 0; i < m_doll.Length; ++i)
        {
            if (m_doll[i].Line == 1) // 아래 라인일 때
            {
                // 좌측 또는 우측 커튼으로 진입 시
                if (m_Dolls[i].transform.position.x <= -8f || m_Dolls[i].transform.position.x >= 8f)
                {
                    Vector3 targetPos = Vector3.zero;
                    if (m_UpLineDir == 1f)
                        targetPos = new Vector3(-5f, 1.35f, 0f);
                    else if (m_UpLineDir == -1f)
                        targetPos = new Vector3(5f, 1.35f, 0f);

                    // 이동할 라인의 시작점에 가까운 인형이랑 최소 거리가 확보되었는지 검사
                    float dist = 10f;
                    for (int j = 0; j < m_doll.Length; ++j)
                    {
                        if (m_doll[j].Line == 2) // 이동하려는 라인 인형일 때
                        {
                            float newDist = Vector3.Distance(targetPos, m_Dolls[j].transform.position);
                            if (dist > newDist)
                                dist = newDist;
                        }
                    }

                    if (dist > 5f) // 확보 상태일 시 이동
                        m_doll[i].Change_Line(2, targetPos);
                }
            }
            else if (m_doll[i].Line == 2) // 상단 라인일 때
            {
                // 좌측 또는 우측 커튼으로 진입 시
                if (m_Dolls[i].transform.position.x <= -5f || m_Dolls[i].transform.position.x >= 5f)
                {
                    Vector3 targetPos = Vector3.zero;
                    if (m_DownLineDir == 1f)
                        targetPos = new Vector3(-8f, -2.1f, 0f);
                    else if (m_DownLineDir == -1f)
                        targetPos = new Vector3(8f, -2.1f, 0f);

                    // 이동할 라인의 시작점에 가까운 인형이랑 최소 거리가 확보되었는지 검사
                    float dist = 10f;
                    for (int j = 0; j < m_doll.Length; ++j)
                    {
                        if (m_doll[j].Line == 1)
                        {
                            float newDist = Vector3.Distance(targetPos, m_Dolls[j].transform.position);
                            if (dist > newDist)
                                dist = newDist;
                        }
                    }

                    if (dist > 5f) // 확보 상태일 시 이동
                        m_doll[i].Change_Line(1, targetPos);
                }
            }
        }
    }

    public void Start_Belt()
    {
        Transform CanvasTransform = GameObject.Find("Canvas").transform;

        for (int i = 0; i < m_Dolls.Length; ++i)
        {
            GameObject clone = Instantiate(m_Hpbar, Vector2.zero, Quaternion.identity, CanvasTransform);
            clone.GetComponent<ShootDollHpbar>().Owner = m_Dolls[i];
            m_doll[i].Hpbar = clone;
        }

        m_useBelt = true;
    }

    public void Over_Game(ShootDoll self = null)
    {
        if (m_overEffect)
            return;

        for (int i = 0; i < m_doll.Length; ++i)
        {
            if(m_doll[i] != self)
                m_doll[i].Explode_Doll();
        }

        m_overEffect = true;
    }
}
