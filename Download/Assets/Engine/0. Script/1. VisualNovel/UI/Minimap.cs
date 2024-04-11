using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform m_MinimapCamera;
    [SerializeField] private Transform m_Owner;
    //[SerializeField] private float m_lerpSpeed = 5.0f;

    public void Update_Minimap()
    {
        m_MinimapCamera.position = new Vector3(m_Owner.position.x, 500.0f, m_Owner.position.z);
        m_MinimapCamera.rotation = Quaternion.Euler(90.0f, m_Owner.eulerAngles.y, 0.0f); // 소유자의 회전을 가져와서 미니맵 카메라의 y 축 회전값으로 설정
        //m_MinimapCamera.rotation = Quaternion.Lerp(m_MinimapCamera.rotation, Quaternion.Euler(90.0f, m_Owner.eulerAngles.y, 0.0f), Time.deltaTime * m_lerpSpeed);
    }
}
