using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootContainerBelt : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject[] m_Dolls;

    [Header("Resource")]
    [SerializeField] private GameObject m_Hpbar;

    private bool m_startBelt = false;
    private float m_speed = 6f;

    private float m_DownLineDir = 1f;
    private float m_DownTime = 0f;
    private float m_DownChang = 0f;
    private float m_DownChangMin = 6f;
    private float m_DownChangMax = 7f;

    private float m_UpLineDir = -1f;
    private float m_UpTime = 0f;
    private float m_UpChang = 3f;
    private float m_UpChangMin = 12f;
    private float m_UpChangMax = 15f;

    private ShootDoll[] m_doll;

    private void Start()
    {
        m_doll = new ShootDoll[m_Dolls.Length];
        for (int i = 0; i < m_Dolls.Length; ++i)
            m_doll[i] = m_Dolls[i].GetComponent<ShootDoll>();

        m_DownChang = Random.Range(m_DownChangMin, m_DownChangMax);
        m_UpChang = m_DownChang;// Random.Range(m_UpChangMin, m_UpChangMax);
    }

    private void Update()
    {
        //if (!m_startBelt)
        //    return;

        m_DownTime += Time.deltaTime;
        if (m_DownTime >= m_DownChang)
        {
            m_DownTime = 0f;
            m_DownChang = Random.Range(m_DownChangMin, m_DownChangMax);

            m_DownLineDir *= -1;
            m_UpLineDir *= -1;
        }

        //m_UpTime += Time.deltaTime;
        //if (m_UpTime >= m_UpChang)
        //{
        //    m_UpTime = 0;
        //    m_UpChang = Random.Range(m_UpChangMin, m_UpChangMax);

        //    m_UpLineDir *= -1;
        //}


        for (int i = 0; i < m_doll.Length; ++i)
        {
            if(m_doll[i].Line == 1)
            {
                m_Dolls[i].transform.Translate((m_DownLineDir * m_speed) * Time.deltaTime, 0, 0);
            }
            else if(m_doll[i].Line == 2)
            {
                m_Dolls[i].transform.Translate((m_UpLineDir * m_speed) * Time.deltaTime, 0, 0);
            }
        }
    }

    private void LateUpdate()
    {
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
                        targetPos = new Vector3(-5f, 1.25f, 0f);
                    else if (m_UpLineDir == -1f)
                        targetPos = new Vector3(5f, 1.25f, 0f);

                    // 이동할 라인의 시작점에 가까운 인형이랑 최소 거리가 확보되었는지 검사
                    float dist = 10f;
                    for (int j = 0; j < m_doll.Length; ++j)
                    {
                        if(m_doll[j].Line == 2) // 이동하려는 라인 인형일 때
                        {
                            float newDist = Vector3.Distance(targetPos, m_Dolls[j].transform.position);
                            if (dist > newDist)
                                dist = newDist;
                        }
                    }

                    if (dist > 5f)
                    {
                        // 확보 상태일 시 이동
                        m_doll[i].Line = 2;
                        m_Dolls[i].transform.position = targetPos;
                    }
                }
            }
            else if (m_doll[i].Line == 2) // 상단 라인일 때
            {
                // 좌측 또는 우측 커튼으로 진입 시
                if (m_Dolls[i].transform.position.x <= -5f || m_Dolls[i].transform.position.x >= 5f)
                {
                    Vector3 targetPos = Vector3.zero;
                    if (m_DownLineDir == 1f)
                        targetPos = new Vector3(-8f, -2f, 0f);
                    else if (m_DownLineDir == -1f)
                        targetPos = new Vector3(8f, -2f, 0f);

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

                    if (dist > 5f)
                    {
                        // 확보 상태일 시 이동
                        m_doll[i].Line = 1;
                        if (m_DownLineDir == 1f)
                            m_Dolls[i].transform.position = new Vector3(-8f, -2f, 0f);
                        else if (m_DownLineDir == -1f)
                            m_Dolls[i].transform.position = new Vector3(8f, -2f, 0f);
                    }
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
        }

        m_startBelt = true;
    }
}
