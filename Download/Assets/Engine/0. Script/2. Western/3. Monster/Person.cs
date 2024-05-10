using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public enum PERSONTYPE
    {
        PT_CITIZEN, PT_CRIMINAL, PT_END
    }

    protected PERSONTYPE m_personType = PERSONTYPE.PT_END;
    protected MeshRenderer m_meshRenderer;

    private Vector3 m_StartPosition;
    private float m_shakeTime = 0.5f;
    private float m_shakeAmount = 2f; // ¼¼±â

    public PERSONTYPE PersonType => m_personType;

    protected Person()
    {
    }

    protected void Initialize()
    {
        m_StartPosition = transform.localPosition;
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Start_Shake()
    {
        StartCoroutine(Shake(m_shakeAmount, m_shakeTime));
    }

    IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            Vector3 randomPoint = m_StartPosition + new Vector3(Random.insideUnitSphere.x, Random.insideUnitSphere.y, 0) * ShakeAmount;//m_StartPosition + Random.insideUnitSphere * ShakeAmount;
            transform.localPosition = Vector3.Lerp(m_StartPosition, randomPoint, Time.deltaTime);
            yield return null;

            timer += Time.deltaTime;
        }
        transform.localPosition = m_StartPosition;
        yield break;
    }
}
