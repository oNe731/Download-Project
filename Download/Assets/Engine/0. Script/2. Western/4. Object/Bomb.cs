using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Bomb : MonoBehaviour
    {
        private KeyCode m_keyType = KeyCode.None;

        private float m_initialSpeed       = 4.5f;   // ∞¯¿« √ ±‚ º”µµ
        private float m_initialbounceForce = 3.5f;   // ∆®±Ê ∂ß¿« √ ±‚ »˚
        private float m_bounceDampening    = 0.5f; // ∆®±Ê ∂ß º”µµ ∞®º” ∫Ò¿≤
        private int   m_maxBounceCount     = 2;    // √÷¥Î ∆®±Ê »Ωºˆ

        private Vector3 m_startPosition;
        private Vector3 m_targetPosition = new Vector3(-0.186f, 0.14f, -53.984f);
        private float   m_bounceForce = 0f;
        private int     m_bounceCount = 0;

        private Rigidbody m_rigidbody;
        private TimeUI m_time = null;
        private float m_timerMax = 2f; // ∆¯≈∫ ≈∏¿Ã∏”¥¬ ∆«≥⁄ ≈∏¿Ã∏”∫∏¥Ÿ ¬™¿Ω
        private float m_timer    = 0f;

        private GameObject m_uiKey = null;
        private GameObject m_uiTime = null;

        private GameObject m_differentBomb = null;
        public Vector3 TargetPosition { set => m_targetPosition = value; }
        public float TimerMax { set => m_timerMax = value; }
        public GameObject DifferentBomb { set => m_differentBomb = value; }

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            m_startPosition = transform.position;
            Initialize_Bomb();
        }

        private void Update()
        {
//#if UNITY_EDITOR
//            if (Input.GetKeyDown(KeyCode.R))
//                Initialize_Bomb();
//#endif

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (m_uiTime != null)
            {
                m_timer -= Time.deltaTime;
                m_time.Update_Time(m_timerMax, m_timer);

                if (m_timer > 0f)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject == gameObject)
                            {
                                Western_Play level = WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>();
                                if (level == null)
                                    return;

                                Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.001f);
                                if (level.TargetUI == null)
                                    level.TargetUI = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/TargetUI"), position, Quaternion.identity);
                                else
                                    level.TargetUI.transform.position = position;

                                level.TargetUI.GetComponent<TargetUI>().Target = hit.collider.gameObject;
                            }
                        }
                    }
                    else if (Input.GetKeyDown(m_keyType))
                    {
                        Western_Play level = WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>();
                        if (level == null)
                            return;

                        if (level.TargetUI == null)
                            return;

                        WesternManager.Instance.IsShoot = true;
                        Is_Destroy();
                        return;
                    }
                }
                else
                {
                    // ∆„
                    Western_Play level = WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>();
                    if (level == null)
                        return;
                    level.Fail_Group();

                    // ¥Ÿ∏• ∆¯∆«¿∫ ªË¡¶
                    if(m_differentBomb != null)
                        Destroy(m_differentBomb);

                    Is_Destroy();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if(m_bounceCount < m_maxBounceCount) // πŸ¥⁄ø° ¥Í¿∏∏È ¿ß∑Œ ∆®±‚¥¬ »˚ ¿˚øÎ
                {
                    if(m_bounceCount == 0) // UI ª˝º∫
                    {
                        if (m_uiKey != null)
                            Destroy(m_uiKey);

                        m_uiKey = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_Key"), GameObject.Find("Canvas").transform);
                        m_uiKey.GetComponent<KeyUI>().Owner = gameObject;
                        m_uiKey.GetComponent<KeyUI>().KeyType = m_keyType;

                        if (m_uiTime != null)
                            Destroy(m_uiTime);

                        m_uiTime = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_Time"), GameObject.Find("Canvas").transform);
                        m_time = m_uiTime.GetComponent<TimeUI>();
                        m_time.Owner = gameObject;
                    }

                    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, m_bounceForce, m_rigidbody.velocity.z);
                    m_bounceForce       *= m_bounceDampening;
                    m_bounceCount++;
                }
                else // √÷¥Î ∆®±Ê »Ωºˆø° µµ¥ﬁ«œ∏È ∏ÿ√„
                {
                    m_rigidbody.velocity = Vector3.zero;
                    m_rigidbody.isKinematic = true;
                }
            }
        }

        private void Initialize_Bomb()
        {
            WesternManager.Instance.IsShoot = false;

            transform.position = m_startPosition;
            m_timer = m_timerMax;

            m_bounceCount = 0;
            m_bounceForce = m_initialbounceForce;

            //Vector3 direction = (m_targetPosition - transform.position).normalized;
            //m_rigidbody.isKinematic = false;
            //m_rigidbody.velocity = Vector3.zero;
            //m_rigidbody.velocity = new Vector3(direction.x * m_initialSpeed, Mathf.Abs(direction.y * m_initialSpeed), direction.z * m_initialSpeed);

            Vector3 direction = (m_targetPosition - transform.position).normalized;
            direction.y = 0;
            direction = direction.normalized;
            m_rigidbody.isKinematic = false;
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.velocity = new Vector3(direction.x * m_initialSpeed, 0, direction.z * m_initialSpeed);


            int index = Random.Range(0, 4);
            switch(index)
            {
                case 0:
                    m_keyType = KeyCode.Q;
                    break;
                case 1:
                    m_keyType = KeyCode.W;
                    break;
                case 2:
                    m_keyType = KeyCode.E;
                    break;
                case 3:
                    m_keyType = KeyCode.R;
                    break;
            }
        }

        private void Is_Destroy()
        {
            if(m_uiKey != null)
                Destroy(m_uiKey);

            if (m_uiTime != null)
                Destroy(m_uiTime);

            Western_Play level = WesternManager.Instance.LevelController.Get_CurrentLevel<Western_Play>();
            if (level != null)
            {
                if (level.TargetUI != null)
                    Destroy(level.TargetUI);
            }

            Destroy(gameObject);
        }
    }
}