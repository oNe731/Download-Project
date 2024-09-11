using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Monster.TYPE m_type;
    [SerializeField] private int m_count;

    private List<Monster> m_monster;
    private Collider[] m_areas;

    private void Awake()
    {
        m_areas = gameObject.GetComponents<Collider>();
    }

    private void Start()
    {
        string path = "5. Prefab/3. Horror/Monster/";
        switch(m_type)
        {
            case Monster.TYPE.TYPE_STRAITJACKER:
                path += "Straitjacket";
                break;

            case Monster.TYPE.TYPE_BUG:
                path += "Bug";
                break;

            case Monster.TYPE.TYPE_BASICUPPERBODY:
                path += "BasicUpperBody";
                break;

            case Monster.TYPE.TYPE_FASTUPPERBODY:
                path += "FastUpperBody";
                break;
        }

        m_monster = new List<Monster>();
        for (int i = 0; i < m_count; ++i)
        {
            GameObject gameObject = GameManager.Ins.Resource.LoadCreate(path);
            if (gameObject == null)
                break;

            Monster monster = gameObject.GetComponent<Monster>();
            if (monster == null)
                break;
            monster.Initialize_Monster(this);
            m_monster.Add(monster);
        }
    }

    private void OnEnable() // 활성화될 때
    {
        if (m_monster == null) return;

        for (int i = 0; i < m_monster.Count; ++i)
        {
            if (m_monster[i] == null) continue;
            m_monster[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable() // 비활성화될 때
    {
        if (m_monster == null) return;

        for (int i = 0; i < m_monster.Count; ++i)
        {
            if (m_monster[i] == null) continue;
            m_monster[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        
    }

    public Vector3 Get_RandomPosition()
    {
        return Get_RandomPointInCollider(m_areas[Random.Range(0, m_areas.Length)]);
    }

    private static Vector3 Get_RandomPointInCollider(Collider collider)
    {
        Vector3 point;
        do
        {
            point = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        } while (!collider.bounds.Contains(point)); // 콜라이더 내에 있는지 확인

        return point;
    }
        
    public bool Check_Position(Vector3 position)
    {
        foreach (var collider in m_areas)
        {
            if (collider.bounds.Contains(position))
                return true;
        }

        return false;
    }
}
