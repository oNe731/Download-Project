using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DOLLTYPE { DT_FAIL, DT_BIRD, DT_SHEEP, DT_CAT, DT_END };

public class ShootDoll : MonoBehaviour
{
    [SerializeField] private GameObject m_Particle;
    [SerializeField] private GameObject m_PanelObject;
    [SerializeField] private GameObject m_ClearObject;

    [SerializeField] private DOLLTYPE m_dollType = DOLLTYPE.DT_END;
    [SerializeField] private int m_line = 0;
    public int Line
    {
        get { return m_line; }
        set { m_line = value; }
    }

    private int m_hp = 5;
    public int Hp
    {
        get { return m_hp; }
        set { m_hp = value; }
    }

    private ShootContainerBelt m_belt;
    public ShootContainerBelt Belt
    {
        set { m_belt = value; }
    }

    private GameObject m_hpbar;
    public GameObject Hpbar
    {
        set { m_hpbar = value; }
    }

    private int m_blinkCount = 2;
    private SpriteRenderer m_spriteRenderer;
    private CapsuleCollider m_collider;
    public CapsuleCollider Collider
    {
        get { return m_collider; }
        set { m_collider = value; }
    }

    private bool m_clear = false;
    private bool m_clearImage = false;
    private float m_clearTime = 0f;
    private bool m_over = false;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (m_clear)
        {
            m_clearTime += Time.deltaTime;
            if (m_clearTime > 1.5f)
            {
                if (!m_belt.OverEffect)
                {
                    m_belt.Over_Game(this); // 2) 1.5초 뒤 본인 제외 인형 전부 폭발
                    Destroy(m_hpbar);       // 본인 hp바 삭제

                }
                else if (!m_clearImage && m_clearTime > 3) // 3) 1.5초 뒤 해당 오브젝트 이미지 화면 가운데 생성
                {
                    m_clearImage = true;

                    m_PanelObject.SetActive(true);
                    m_ClearObject.SetActive(true);
                    m_ClearObject.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                }
                else if (!m_over && m_clearTime > 4.5) // 4) 1.5초 뒤 페이드 아웃으로 전환
                {
                    m_over = true;
                    UIManager.Instance.Start_FadeOut(1f, Color.black, () => VisualNovelManager.Instance.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_NOVELEND), 0.5f, false);
                }
            }
        }
        else
        {
            if (m_line == 1) // 아래 라인 좌측 또는 우측으로 이동했을 시
            {
                if (transform.position.x >= -6f && transform.position.x <= 6f)
                    m_collider.enabled = true;
                else
                    m_collider.enabled = false;
            }
            else if (m_line == 2) // 상단 라인 좌측 또는 우측으로 이동했을 시
            {
                if (transform.position.x >= -3f && transform.position.x <= 3f)
                    m_collider.enabled = true;
                else
                    m_collider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ball(Clone)")
        {
            m_hp--;
            if(m_hp <= 0) // 게임 종료
            {
                VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Shoot>().ShootGameStop = true;
                VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Shoot>().DollType = m_dollType;
                m_belt.UseBelt = false; // 1) 인형 일시 정지
                m_clear = true;
            }
            else
            {
                StartCoroutine(Blink());
            }

            other.gameObject.GetComponent<ShootBall>().Arrived();

            Destroy(other.gameObject);
            Create_Particle();
        }
    }

    private IEnumerator Blink()
    {
        Color startColor = m_spriteRenderer.color;

        int Count = 0;
        while (Count < m_blinkCount)
        {
            float alpha = startColor.a;
            while (alpha > 0f)
            {
                alpha -= 0.1f;
                m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, alpha);
                yield return new WaitForSeconds(0.005f);
            }
            yield return new WaitForSeconds(0.01f);

            while (alpha < 1f)
            {
                alpha += 0.1f;
                m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, alpha);
                yield return new WaitForSeconds(0.005f);
            }
            yield return new WaitForSeconds(0.01f);

            Count++;
        }
        m_spriteRenderer.color = startColor;
    }

    private void Create_Particle()
    {
        GameObject clone = Instantiate(m_Particle);
        clone.transform.position = transform.position;
    }

    public void Explode_Doll()
    {
        Create_Particle();
        Destroy(gameObject);
    }

    public void Change_Line(int line, Vector3 targetPosition)
    {
        m_line = line;
        transform.position = targetPosition;
    }
}
