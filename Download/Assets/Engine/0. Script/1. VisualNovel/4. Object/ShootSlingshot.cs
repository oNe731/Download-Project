using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootSlingshot : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject m_gauge;
    [SerializeField] private GameObject m_ball;

    [Header("Resource")]
    [SerializeField] private Sprite[] m_Image;

    [Header("Value")]
    [SerializeField] private float m_curSpeed = 0.0f;
    [SerializeField] private float m_minSpeed = 3.0f;
    [SerializeField] private float m_maxSpeed = 8.0f;
    [SerializeField] private float m_gaugeSpeed = 7.0f;

    private SpriteRenderer m_spriteRenderer;
    private Slider m_barSlider;
    private bool m_Up = true;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_barSlider = m_gauge.GetComponent<Slider>();

        m_curSpeed = m_minSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // 마우스의 현재 위치를 가져옴
            Vector3 mousePosition = Input.mousePosition;

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
        else if (Input.GetMouseButtonUp(0))
        {
            GameObject ball = Instantiate(m_ball);
            ball.GetComponent<ShootBall>().TargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            ball.GetComponent<ShootBall>().Speed = m_curSpeed;

            m_spriteRenderer.sprite = m_Image[0];

            m_curSpeed = m_minSpeed;
            m_barSlider.value = m_curSpeed;
        }
    }
}
