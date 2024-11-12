using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Western;

public class Obstacle : MonoBehaviour
{
    public enum TYPE { TP_CAT, TP_MOM, TP_AYAKA, TP_END }
    public enum STATE { SP_NONE, SP_END }

    [SerializeField] private TYPE m_type;
    private STATE m_state = STATE.SP_NONE;
    private float m_dist = 3f;

    private Western_PlayLv2 m_level;
    private WalkPlayer m_player;

    private Quaternion m_wakeUpQuaternion;
    private float m_wakeUpRotationSpeed = 2f;

    private void Start()
    {
        m_wakeUpQuaternion = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        m_level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv2>();
        if(m_level != null)
            m_player = m_level.Player;
    }

    private void Update()
    {
        switch(m_state)
        {
            case STATE.SP_NONE:
                float distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);
                if (distanceToPlayer <= m_dist)
                {
                    m_player.Set_Lock(true);
                    switch (m_type)
                    {
                        case TYPE.TP_CAT:
                            StartCoroutine(WakeUp());
                            break;

                        case TYPE.TP_MOM:
                            StartCoroutine(WakeUp());
                            break;

                        case TYPE.TP_AYAKA:
                            GetComponent<AudioSource>().Play();
                            StartCoroutine(WakeDown());
                            StartCoroutine(RotateChild());
                            break;
                    }
                    m_state = STATE.SP_END;
                }
                break;
        }
    }

    private IEnumerator WakeUp()
    {
        // 사운드 재생
        // GameManager.Ins.Sound.Play_AudioSource(audioSource, "Western_Panel_Rotation", false, 1f);

        bool talk = false;
        float time = 0f;

        while (transform.rotation != m_wakeUpQuaternion)
        {
            if(talk == false)
            {
                time += Time.deltaTime;
                if (time > 1f)
                {
                    talk = true;
                    Active_Text();
                }
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, m_wakeUpQuaternion, m_wakeUpRotationSpeed * Time.deltaTime);
            yield return null;
        }

        if (talk == false)
            Active_Text();
        yield break;
    }

    private IEnumerator WakeDown()
    {
        bool talk = false;
        float time = 0f;

        Vector3 targetPosition = transform.position + new Vector3(0, -1.6f, 0);
        while (transform.rotation != m_wakeUpQuaternion || Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            if (talk == false)
            {
                time += Time.deltaTime;
                if (time > 1f)
                {
                    talk = true;
                    Active_Text();
                }
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, 3f * Time.deltaTime);
            yield return null;
        }

        if(talk == false)
            Active_Text();
        yield break;
    }

    private IEnumerator RotateChild()
    {
        Transform child = transform.GetChild(0);
        float angle = 65f;
        bool increasing = true;

        while (true)
        {
            // 각도 증가 또는 감소
            if (increasing)
            {
                angle += Time.deltaTime * 10f; // 흔들림 속도
                if (angle >= 100f) // -
                    increasing = false;
            }
            else
            {
                angle -= Time.deltaTime * 10f;
                if (angle <= 80f) // +
                    increasing = true;
            }

            // 자식 오브젝트의 X축 회전 적용
            child.localRotation = Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
    }

    private void Active_Text()
    {
        string[] texts;
        string[] sounds;
        float horizontal;
        float height;
        switch (m_type)
        {
            case TYPE.TP_CAT:
                horizontal = -0.05f;
                height = 2.2f;
                texts = new string[1];
                texts[0] = "*야옹";
                sounds = new string[1];
                sounds[0] = "Western_Criminal_Caught";
                Create_SpeechBubble(texts, sounds, horizontal, height);
                break;

            case TYPE.TP_MOM:
                horizontal = 0f;
                height = 2.5f;
                texts = new string[10];
                texts[0] = "어머...";
                texts[1] = "무슨 일이니?";
                texts[2] = "손에 들고 있는 건...";
                texts[3] = "괜찮니 얘야?";
                texts[4] = "...";
                texts[5] = "진정하렴.";
                texts[6] = "아가야...";
                texts[7] = "이러지마.";
                texts[8] = "...";
                texts[9] = "*우는 소리";
                sounds = new string[10];
                sounds[0] = "";
                sounds[1] = "";
                sounds[2] = "";
                sounds[3] = "";
                sounds[4] = "";
                sounds[5] = "";
                sounds[6] = "";
                sounds[7] = "";
                sounds[8] = "";
                sounds[9] = "Western_WomanCry";
                Create_SpeechBubble(texts, sounds, horizontal, height);
                break;

            case TYPE.TP_AYAKA:
                horizontal = 0f;
                height = 4.5f; // 4 5
                texts = new string[8];
                texts[0] = "o....";
                texts[1] = "....욱";
                texts[2] = "...";
                texts[3] = "살려줘...";
                texts[4] = "...끄으...";
                texts[5] = "컥...";
                texts[6] = "...";
                texts[7] = "*고통에 몸부림 치는 소리";
                sounds = new string[1];
                sounds[0] = "";
                sounds[1] = "";
                sounds[2] = "";
                sounds[3] = "";
                sounds[4] = "";
                sounds[5] = "";
                sounds[6] = "";
                sounds[7] = "";
                Create_SpeechBubble(texts, sounds, horizontal, height);
                break;
        }
    }

    private void Create_SpeechBubble(string[] texts, string[] m_sounds, float horizontal, float height)
    {
        if (texts == null)
            return;

        // 말풍선 UI
        GameObject uiObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/UI/DialogBox", GameObject.Find("Canvas").transform);
        uiObject.GetComponent<Transform>().position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(horizontal, height, 0f));
        uiObject.GetComponent<ObstacleDialog>().Start_Dialog(texts, m_sounds, 0f);
    }

    public void Attacked_Obstacle()
    {
        AudioSource[] audioSource = transform.GetChild(0).GetComponents<AudioSource>();
        if(audioSource != null)
        {
            for(int i = 0; i < audioSource.Length; ++i)
                audioSource[i].Play();
        }

        if(m_type == TYPE.TP_CAT)
        {
            // 판넬 변화
            transform.GetChild(0).GetComponent<MeshRenderer>().material = GameManager.Ins.Resource.Load<Material>("1. Graphic/3D/2. Western/Character/Round2/Western_Atteck/2_Pannel_Cat_2");

            // 주인공 대사 출력
            string[] texts = new string[4];
            texts[0] = "고양이 목숨은 9개라고.....";
            texts[1] = "허무하군! 불쌍한 야옹이 같으니라고.";
            texts[2] = "하지만 나로서도 어쩔 수가 없어. 이런 의뢰는 처음이야. 고양이 친구.";
            texts[3] = "애도하지.....";
            m_level.PlayerDialog.Start_Dialog(texts, 2f);
        }
        else if (m_type == TYPE.TP_MOM)
        {
            // 판넬 변화
            transform.GetChild(0).GetComponent<MeshRenderer>().material = GameManager.Ins.Resource.Load<Material>("1. Graphic/3D/2. Western/Character/Round2/Western_Atteck/2_Pannel_Mom_2");

            // 비명소리
            GameManager.Ins.Sound.Play_AudioSource(GetComponent<AudioSource>(), "Western_WomanScream", false, 1f);
        }
        else if(m_type == TYPE.TP_AYAKA)
        {
            // 판넬 변화
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = GameManager.Ins.Resource.Load<Material>("1. Graphic/3D/2. Western/Character/Round2/Western_Atteck/2_Pannel_Ayaka_2");
            transform.GetChild(1).gameObject.SetActive(true);

            // 주인공 대사 출력
            string[] texts = new string[2];
            texts[0] = "도대체 이런 안타까운 희생을 해야하는 이유가 뭐야?";
            texts[1] = "이런, 이젠 정말 아무것도 모르겠군.";
            m_level.PlayerDialog.Start_Dialog(texts, 1f);
        }

        // 초상화 업데이트
        m_level.PlayerPortrait.Change_Portrait();

        // 메모지 업데이트
        m_level.PlayerMemo.Check_List();

        // 완료 시 앞으로 전진 가능
        StartCoroutine(Wait_Dialog());
    }

    private IEnumerator Wait_Dialog()
    {
        while(true)
        {
            if (m_level.PlayerDialog.IsUpdate == false)
                break;
            yield return null;
        }

        m_player.Set_Lock(false);
        yield break;
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_dist);
#endif
    }

}
