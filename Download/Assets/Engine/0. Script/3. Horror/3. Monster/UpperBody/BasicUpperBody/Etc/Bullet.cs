using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 m_direction;
    private float m_damage;
    private float m_speed;

    private float m_time = 0f;

    public void Initialize_Bullet(Vector3 startPosition, Vector3 targetPosition, float damage, float speed)
    {
        gameObject.transform.position = startPosition;
        m_direction = (targetPosition - startPosition).normalized;
        m_damage = damage;
        m_speed = speed;
    }

    void Update()
    {
        m_time += Time.deltaTime;
        if(m_time >= 5f)
            Destroy(gameObject);

        transform.position += m_direction * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Ins.Horror.Player.Damage_Player(m_damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Ceiling"))
        {
            Destroy(gameObject);
        }
    }
}
