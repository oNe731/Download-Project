using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootSlingshot : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject m_gauge;
    [SerializeField] private GameObject m_ball;
    [SerializeField] private GameObject m_target;

    [Header("Resource")]
    [SerializeField] private Sprite[] m_Image;

    [Header("Value")]
    [SerializeField] private float m_curSpeed = 0.0f;
    [SerializeField] private float m_minSpeed = 3.0f;
    [SerializeField] private float m_maxSpeed = 8.0f;
    [SerializeField] private float m_gaugeSpeed = 7.0f;

    [SerializeField] private float m_shakeTime   = 0.2f;
    [SerializeField] private float m_shakeAmount = 10.0f; // 세기

    private Vector3 m_StartPosition;
    private SpriteRenderer m_spriteRenderer;
    private Slider m_barSlider;
    private bool m_Up = true;

    private bool m_use = true;
    public bool Use
    {
        set { m_use = value; }
    }

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_barSlider = m_gauge.GetComponent<Slider>();

        m_curSpeed = m_minSpeed;
        m_StartPosition = transform.position;
    }

    private void Update()
    {
        if (!VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Shoot>().ShootGameStart || VisualNovelManager.Instance.LevelController.Get_CurrentLevel<Novel_Shoot>().ShootGameStop)
            return;

        if (m_use)
            Ok_Use();
        else
            No_Use();
    }

    private void Ok_Use()
    {
        if (Input.GetMouseButton(0))
        {
            // 마우스의 현재 위치를 가져옴
            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.y <= 255f)
            {
                Reset_Use();
                StartCoroutine(Shake(m_shakeAmount, m_shakeTime));
                return;
            }

            // 화면의 가로 길이를 3등분하여 각 섹션의 경계를 계산
            float screenWidth = Screen.width;
            float sectionWidth = screenWidth / 3f;

            // 마우스의 x 좌표를 섹션에 따라 분류
            int section = (int)(mousePosition.x / sectionWidth);
            switch (section)
            {
                case 0:
                    m_spriteRenderer.sprite = m_Image[3];
                    break;
                case 1:
                    m_spriteRenderer.sprite = m_Image[2];
                    break;
                case 2:
                    m_spriteRenderer.sprite = m_Image[1];
                    break;
                default:
                    break;
            }

            if (m_Up)
            {
                m_curSpeed += Time.deltaTime * m_gaugeSpeed;
                if (m_curSpeed >= m_maxSpeed)
                    m_Up = false;
            }
            else
            {
                m_curSpeed -= Time.deltaTime * m_gaugeSpeed;
                if (m_curSpeed <= m_minSpeed)
                    m_Up = true;
            }
            m_barSlider.value = m_curSpeed;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 NewPosition = Vector3.zero;

            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.y <= 255f)
            {
                Reset_Use();
                StartCoroutine(Shake(m_shakeAmount, m_shakeTime));
                return;
            }

            float screenWidth = Screen.width;
            float sectionWidth = screenWidth / 3f;
            int section = (int)(mousePosition.x / sectionWidth);
            switch (section)
            {
                case 0:
                    NewPosition = new Vector3(7.948f, -4.5f, 0f);
                    break;
                case 1:
                    NewPosition = new Vector3(6.861f, -4.5f, 0f);
                    break;
                case 2:
                    NewPosition = new Vector3(5.7f, -4.5f, 0f);
                    break;
            }

            Vector3 targetPosition =  Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));
            Transform canvasTransform = GameObject.Find("Canvas").transform;

            GameObject targetUI = Instantiate(m_target, Vector2.zero, Quaternion.identity, canvasTransform);
            targetUI.GetComponent<Transform>().position = Camera.main.WorldToScreenPoint(targetPosition);

            GameObject ballObject = Instantiate(m_ball);
            ballObject.GetComponent<Transform>().position = NewPosition;

            ShootBall ball = ballObject.GetComponent<ShootBall>();
            ball.Owner = this;
            ball.TargetUI = targetUI;
            ball.TargetPosition = targetPosition;
            ball.Speed = m_curSpeed;

            Reset_Use();
            m_use = false;
        }
    }

    private void No_Use()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shake(m_shakeAmount, m_shakeTime));
        }
    }

    private void Reset_Use()
    {
        m_spriteRenderer.sprite = m_Image[0];
        m_curSpeed = m_minSpeed;
        m_barSlider.value = m_curSpeed;
    }

    IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            Vector3 randomPoint = m_StartPosition + Random.insideUnitSphere * m_shakeAmount;
            transform.localPosition = Vector3.Lerp(m_StartPosition, randomPoint, Time.deltaTime);
            yield return null;

            timer += Time.deltaTime;
        }
        transform.localPosition = m_StartPosition;
        yield break;
    }
}