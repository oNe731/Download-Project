using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Bomb : MonoBehaviour
    {
        public enum TYPE { TP_TUTORIAL, TP_PLAY, TP_END }
        public enum ORDER { OD_FIRST, OD_SECOND, OD_END };

        private TYPE m_type = TYPE.TP_PLAY;
        private ORDER m_order = ORDER.OD_END;
        private KeyCode m_keyType = KeyCode.None;

        private float m_initialSpeed       = 4.5f;   // °øÀÇ ÃÊ±â ¼Óµµ
        private float m_initialbounceForce = 3.5f;   // Æ¨±æ ¶§ÀÇ ÃÊ±â Èû
        private float m_bounceDampening    = 0.5f; // Æ¨±æ ¶§ ¼Óµµ °¨¼Ó ºñÀ²
        private int   m_maxBounceCount     = 2;    // ÃÖ´ë Æ¨±æ È½¼ö

        private Vector3 m_startPosition;
        private Vector3 m_targetPosition = new Vector3(-0.186f, 0.14f, -53.984f);
        private float   m_bounceForce = 0f;
        private int     m_bounceCount = 0;

        private Rigidbody m_rigidbody;
        private TimeUI m_time = null;
        private float  m_timerMax = 2f; // ÆøÅº Å¸ÀÌ¸Ó´Â ÆÇ³Ú Å¸ÀÌ¸Óº¸´Ù ÂªÀ½
        private float  m_timer    = 0f;

        private GameObject m_uiKey = null;
        private GameObject m_uiTime = null;

        // Ã¹ ¹øÂ° ÆøÅºÀÌ µÎ¹ø Â° ÆøÅº º¸À¯
        private GameObject m_differentBomb = null;
        public Vector3 TargetPosition { set => m_targetPosition = value; }
        public float TimerMax { set => m_timerMax = value; }
        public TYPE BombType { set => m_type = value; }
        public GameObject DifferentBomb { set => m_differentBomb = value; }
        public ORDER Order { set => m_order = value; }
        public KeyCode KeyType => m_keyType;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            m_startPosition = transform.position;
            Initialize_Bomb();
        }

        private void Initialize_Bomb()
        {
            GameManager.Ins.Western.IsShoot = false;

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

            Set_KeyCode(Random.Range(0, 4));
            if (m_differentBomb != null) // µÎ ¹øÂ° ÆøÅºÀÌ¶û ´ÜÃàÅ° ´Ù¸£°Ô »ç¿ë
            {
                Bomb scirpt = m_differentBomb.GetComponent<Bomb>();
                while (true)
                {
                    if (scirpt.KeyType != m_keyType)
                        break;

                    Set_KeyCode(Random.Range(0, 4));
                }
            }
        }

        private void Set_KeyCode(int index)
        {
            switch (index)
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


        private void Update()
        {
            if (GameManager.Ins.IsGame == false)
            {
                m_rigidbody.isKinematic = true;
                return;
            }
            else
            {
                m_rigidbody.isKinematic = false;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (m_uiTime != null)
            {
                m_timer -= Time.deltaTime;
                m_time.Update_Time(m_timerMax, m_timer);

                if (m_type == TYPE.TP_PLAY)
                    Update_Play();
                else if (m_type == TYPE.TP_TUTORIAL)
                    Update_Tutorial();
            }
        }

        private void Update_Play()
        {
            if (m_timer > 0f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Click_Bomb();
                }
                else if (Input.GetKeyDown(m_keyType))
                {
                    Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
                    if (level == null || level.TargetUI == null)
                        return;

                    if (m_order != ORDER.OD_FIRST)
                        GameManager.Ins.Western.IsShoot = true;

                    level.Gun.Shoot_Gun();
                    Is_Destroy(true);
                    //Debug.Log("Á¦°Å ¼º°ø");
                }
            }
            else // Æã
            {
                Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
                if (level == null)
                    return;
                level.Fail_Group();

                // ´Ù¸¥ ÆøÆÇÀº »èÁ¦
                if (m_differentBomb != null)
                    Destroy(m_differentBomb);

                Is_Destroy(false);
                //Debug.Log("½Ã°£ ¿À¹Ù");
            }
        }

        private void Update_Tutorial()
        {
            if (m_timer > 0f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Click_Bomb();
                }
                // ÆøÅº ¸ÂÃèÀ» ½Ã
                else if (Input.GetKeyDown(m_keyType))
                {
                    Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
                    if (level == null || level.TargetUI == null)
                        return;

                    level.Gun.Shoot_Gun();

                    // ¼º°ø ´ÙÀÌ¾ó·Î±× Ãâ·Â
                    level.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_BombSuccess"));
                    Is_Destroy(true);
                }
            }
            // ÆøÅº ¸ø ¸ÂÃèÀ» ½Ã
            else
            {
                Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
                if (level == null)
                    return;

                // ½ÇÆÐ ´ÙÀÌ¾ó·Î±× Ãâ·Â
                level.DialogPlay.GetComponent<Dialog_PlayWT>().Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>("4. Data/2. Western/Dialog/Play/Round1/Dialog1_Tutorial_BombFail"));
                Is_Destroy(false);
            }
        }

        private void Click_Bomb()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
                    if (level == null)
                        return;

                    Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 0.001f);
                    if (level.TargetUI == null)
                        level.TargetUI = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/TargetUI", position, Quaternion.identity);
                    else
                        level.TargetUI.transform.position = position;

                    level.TargetUI.GetComponent<TargetUI>().Target = hit.collider.gameObject;
                    level.Gun.Click_Gun();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if(m_bounceCount < m_maxBounceCount) // ¹Ù´Ú¿¡ ´êÀ¸¸é À§·Î Æ¨±â´Â Èû Àû¿ë
                {
                    if(m_bounceCount == 0) // UI »ý¼º
                    {
                        if (m_uiKey != null)
                            Destroy(m_uiKey);

                        m_uiKey = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/UI_Key", GameObject.Find("Canvas").transform);
                        m_uiKey.GetComponent<KeyUI>().Owner = gameObject;
                        m_uiKey.GetComponent<KeyUI>().KeyType = m_keyType;

                        if (m_uiTime != null)
                            Destroy(m_uiTime);

                        m_uiTime = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/UI_Time", GameObject.Find("Canvas").transform);
                        m_time = m_uiTime.GetComponent<TimeUI>();
                        m_time.Owner = gameObject;
                    }

                    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, m_bounceForce, m_rigidbody.velocity.z);
                    m_bounceForce       *= m_bounceDampening;
                    m_bounceCount++;
                }
                else // ÃÖ´ë Æ¨±æ È½¼ö¿¡ µµ´ÞÇÏ¸é ¸ØÃã
                {
                    m_rigidbody.velocity = Vector3.zero;
                    m_rigidbody.isKinematic = true;
                }
            }
        }


        private void Is_Destroy(bool success)
        {
            if(m_uiKey != null)
                Destroy(m_uiKey);

            if (m_uiTime != null)
                Destroy(m_uiTime);

            Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
            if (level != null)
            {
                if (level.TargetUI != null)
                    Destroy(level.TargetUI);
            }

            if(success == true)
                Destroy(gameObject);
            else
                StartCoroutine(Wait_PlaySound());
        }

        IEnumerator Wait_PlaySound()
        {
            GetComponent<MeshRenderer>().enabled = false; // ¸Þ½Ã ºñÈ°¼ºÈ­

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);

            Destroy(gameObject);
        }
    }
}