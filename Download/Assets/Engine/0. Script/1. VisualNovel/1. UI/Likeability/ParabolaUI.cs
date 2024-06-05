using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaUI : MonoBehaviour
{
    [Header("[ SHAKE ]")]
    [SerializeField] protected float m_minShakeTime = 0.5f;
    [SerializeField] protected float m_maxShakeTime = 1f;
    [SerializeField] protected float m_minShakeAmount = 500f;
    [SerializeField] protected float m_maxShakeAmount = 1000f;

    [Header("[ PARABOLA ]")]
    [SerializeField] protected float m_minTargetX = -900f;
    [SerializeField] protected float m_maxTargetX = 900f;
    [SerializeField] protected float m_setTargetY = -700f;
    [SerializeField] protected float m_minHeight = 450f;
    [SerializeField] protected float m_maxHeight = 550f;
    [SerializeField] protected float m_minMoveSpeed = 15f;
    [SerializeField] protected float m_maxMoveSpeed = 20f;

    [Header("[ ROTATION ]")]
    [SerializeField] protected bool  m_rotation = false;
    [SerializeField] protected float m_minAngle = -70f;
    [SerializeField] protected float m_maxAngle = 70f;
    [SerializeField] protected float m_minRotationSpeed = -70f;
    [SerializeField] protected float m_maxRotationSpeed = 70f;

    protected float m_shakeTime = 0.5f;
    protected float m_shakeAmount = 1000f;

    protected bool    m_down = false;
    protected Vector3 m_targetPosition;
    protected float   m_heightArc; // 포물선의 높이
    protected float   m_moveSpeed; // 이동 속도

    protected Quaternion m_startRotation;
    protected Quaternion m_targetRotation;
    protected float      m_rotationSpeed = 500.0f; // 회전 속도

    private RectTransform m_rectTransform  = null;
    private Vector3       m_startPosition;
    private Coroutine     m_shakeCoroutine = null;

    public bool Down { set => m_down = value; }

    public void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_startPosition = m_rectTransform.anchoredPosition;
        m_startRotation = m_rectTransform.rotation;

        Set_Value();
    }

    private void Set_Value()
    {
        m_shakeTime   = UnityEngine.Random.Range(m_minShakeTime, m_maxShakeTime);
        m_shakeAmount = UnityEngine.Random.Range(m_minShakeAmount, m_maxShakeAmount);

        m_targetPosition = new Vector3(UnityEngine.Random.Range(m_minTargetX, m_maxTargetX), m_setTargetY, 0);
        m_heightArc = UnityEngine.Random.Range(m_minHeight, m_maxHeight);
        m_moveSpeed = Mathf.Lerp(m_minMoveSpeed, m_maxMoveSpeed, (m_heightArc - m_minHeight) / (m_maxHeight - m_minHeight));
        float angle = Vector3.Angle((m_startPosition - m_targetPosition).normalized, Vector3.up); // 각도에 따른 속도 조절
        if (angle < 90)
            angle = 90 + (90 - angle);
        float result = (90 - Mathf.Abs(angle - 90));
        m_moveSpeed *= result;

        m_targetRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(m_minAngle, m_maxAngle));
        m_rotationSpeed  = UnityEngine.Random.Range(m_minRotationSpeed, m_maxRotationSpeed);

        //Debug.Log("TargetPosition : " + m_targetPosition);
        //Debug.Log("minAngle : " + m_minAngle + "/" + "maxAngle : " + m_maxAngle);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    m_rectTransform.anchoredPosition = m_startPosition;
        //    m_rectTransform.rotation = m_startRotation;
        //    Set_Value();
        //}

        if (m_down == false) return;

        float nextX = Mathf.MoveTowards(m_rectTransform.anchoredPosition.x, m_targetPosition.x, m_moveSpeed * Time.deltaTime);
        float distance = m_targetPosition.x - m_startPosition.x;
        float baseY = Mathf.Lerp(m_startPosition.y, m_targetPosition.y, (nextX - m_startPosition.x) / distance);
        float arc = m_heightArc * (nextX - m_startPosition.x) * (nextX - m_targetPosition.x) / (-0.25f * distance * distance);
        m_rectTransform.anchoredPosition = new Vector2(nextX, baseY + arc);

        // 회전
        if(m_rotation == true)
        {
            float step = m_rotationSpeed * Time.deltaTime;
            m_rectTransform.rotation = Quaternion.RotateTowards(m_rectTransform.rotation, m_targetRotation, step);
        }

        // 다 떨어지면 오브젝트 삭제
        if (Mathf.Approximately(nextX, m_targetPosition.x) && Mathf.Approximately(baseY + arc, m_targetPosition.y)) { Destroy(gameObject); }
    }

    public void Shake_Object(Action isAction = null)
    {
        if (gameObject.activeSelf == false) return;

        if (m_shakeCoroutine != null)
            StopCoroutine(m_shakeCoroutine);
        m_shakeCoroutine = StartCoroutine(Shake(isAction));
    }

    public IEnumerator Shake(Action isAction)
    {
        float timer = 0;
        while (timer <= m_shakeTime)
        {
            timer += Time.deltaTime;

            Vector3 randomPoint = m_startPosition + UnityEngine.Random.insideUnitSphere * m_shakeAmount;
            m_rectTransform.anchoredPosition = Vector2.Lerp(m_startPosition, randomPoint, Time.deltaTime);
            yield return null;
        }

        m_rectTransform.anchoredPosition = m_startPosition;

        if (isAction != null)
            isAction?.Invoke();

        m_down = true;

        yield break;
    }
}
