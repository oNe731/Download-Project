using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Western;

public class WalkPlayer : MonoBehaviour
{
    [SerializeField] private Transform[] m_transform;

    protected GameObject m_targetUI = null;
    private Gun m_gun;
    public PlayerStatusBar m_playerStatus;

    private bool m_isLock = false;

    private float m_moveSpeed = 4f;   // �̵� �ӵ�
    private float m_bounceSpeed = 7f;     // ���� �ӵ�
    private float m_bounceHeight = 0.01f; // ���Ʒ� ������ ����
    public float m_returnSpeed = 15f;    // �⺻ ���̷� �����ϴ� �ӵ�

    private float m_bounceTimer = 0f;
    private float m_currentHeight;
    private float m_startHeight;

    private Vector3 m_startPosition;

    private AudioSource m_audioSource;
    private float m_soundTime = 0f;

    public float StartHeight { set => m_startHeight = value; }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

        m_startPosition = transform.position;
        m_startHeight = m_startPosition.y;
        m_currentHeight = m_startPosition.y;

        Western_PlayLv2 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv2>();
        if (level != null)
        {
            m_gun = level.Gun;
            m_playerStatus = level.PlayerStatus;
        }
    }

    private void Update()
    {
        // ����
        if(GameManager.Ins.Western.IsShoot == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Person"))
                    {
                        m_gun.Click_Gun();

                        Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.collider.gameObject.transform.position.z - 0.005f);
                        if (m_targetUI == null)
                            m_targetUI = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/TargetUI", position, Quaternion.identity);
                        else
                            m_targetUI.transform.position = position;

                        m_targetUI.GetComponent<TargetUI>().Target = hit.collider.gameObject;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_targetUI == null)
                    return;

                GameManager.Ins.Western.IsShoot = false;

                Obstacle obstacle = m_targetUI.GetComponent<TargetUI>().Target.GetComponent<Obstacle>();
                if (obstacle != null)
                    obstacle.Attacked_Obstacle();

                m_gun.Shoot_Gun();

                // �Ͼ�� ȭ������ ��½ ȿ�� ���� (������ �ѹ��� ������°� ������ ������)
                GameManager.Ins.UI.Start_FadeIn(0.3f, Color.white);

                // ����Ʈ ����
                GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/1Stage/Effect/Person_Effect", m_targetUI.transform.position, Quaternion.identity);

                // �Ѿ��ڱ� ������Ʈ ����.
                // GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/BulletMarkUI", m_targetUI.transform.position, Quaternion.identity, m_targetUI.GetComponent<TargetUI>().Target.transform);
                GameManager.Ins.Resource.Destroy(m_targetUI);
            }
        }
        else
        {
            if (m_isLock == true)
                return;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                Play_WalkSound();
                m_playerStatus.Update_Value(m_startPosition, transform.position);

                // ����
                transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);

                // ����
                m_bounceTimer += m_bounceSpeed * Time.deltaTime;
                float bounceOffset = Mathf.Sin(m_bounceTimer) * m_bounceHeight;
                m_currentHeight = m_startHeight + bounceOffset;
            }
            else
            {
                m_playerStatus.Stop_Value();

                // ���� ����
                m_currentHeight = Mathf.Lerp(m_currentHeight, m_startHeight, m_returnSpeed * Time.deltaTime);
            }

            transform.position = new Vector3(transform.position.x, m_currentHeight, transform.position.z);
        }
    }

    public void Set_Lock(bool locks)
    {
        m_isLock = locks;

        if(m_playerStatus != null)
            m_playerStatus.Stop_Value();
    }

    protected void Play_WalkSound()
    {
        m_soundTime += Time.deltaTime;
        if (m_soundTime >= 0.5f)
        {
            m_soundTime = 0f;

            int Index = Random.Range(1, 6);
            GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Western_Walk" + Index.ToString(), false, 1f);
        }
    }
}
