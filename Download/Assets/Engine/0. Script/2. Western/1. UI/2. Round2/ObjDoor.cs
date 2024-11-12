using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Western;

public class ObjDoor : MonoBehaviour
{
    private float m_dist = 1f;
    private bool m_isOpen = false;

    private WalkPlayer m_player;

    private void Start()
    {
        Western_PlayLv2 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv2>();
        if (level != null)
            m_player = level.Player;
    }

    private void Update()
    {
        if(m_isOpen == false)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);
            if (distanceToPlayer <= m_dist)
            {
                m_isOpen = true;
                StartCoroutine(Move_Door());
            }
        }
    }

    private IEnumerator Move_Door()
    {
        GetComponent<AudioSource>().Play();

        bool isLock = false;
        m_player.Set_Lock(true);

        float duration = 1f;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 120f, 0f);

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if(isLock == false && elapsedTime >= 0.5f)
                m_player.Set_Lock(false);

            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.rotation = endRotation;
        yield break;
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_dist);
#endif
    }
}
