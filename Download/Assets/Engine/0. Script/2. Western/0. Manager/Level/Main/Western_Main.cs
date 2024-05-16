using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Western_Main : Level
{
    protected RectTransform m_rectTransform;

    protected bool m_dialogStart = false;
    protected bool m_moveGun = false;
    protected bool m_shootGun = false;

    protected Vector3 m_startPosition;
    protected Vector3 m_targetPosition;
    protected float m_time = 0f;
    protected float m_moveDuration = 1f;
    protected float m_shootDuration = 0.3f;
    protected float m_darkDuration = 1f;
    protected int m_shootCount = 0;

    public Western_Main(LevelController levelController) : base(levelController)
    {
        m_rectTransform = WesternManager.Instance.PlayButton.GetComponent<RectTransform>();
        m_startPosition = new Vector3(0f, 0f, 0f);
        m_targetPosition = new Vector3(720f, -200f, 0f);
    }

    protected abstract void Start_Dialog();

    public override void Enter_Level()
    {
        WesternManager.Instance.MainPanel.SetActive(true);
        WesternManager.Instance.PlayButton.GetComponent<Button>().interactable = false;
        UIManager.Instance.Start_FadeIn(1f, Color.black, () => Start_Dialog());
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
        if (m_moveGun)
            Move_Gun();
        else if (m_shootGun)
            Shoot_Gun();
    }

    public override void LateUpdate_Level()
    {
        // 버튼 활성화
        if (m_dialogStart && WesternManager.Instance.DialogPlay.Active == false)
        {
            m_dialogStart = false;
            WesternManager.Instance.PlayButton.GetComponent<Button>().interactable = true;
        }
    }

    public override void Exit_Level()
    {
        WesternManager.Instance.MainPanel.SetActive(false);
    }

    public void Button_Play()
    {
        m_moveGun = true;
    }

    private void Move_Gun()
    {
        // 총 버튼이 대각선으로 화면 밖으로 나감.
        m_time += Time.deltaTime;
        if (m_time >= m_moveDuration)
        {
            m_time = 0f;
            m_rectTransform.anchoredPosition = m_targetPosition;
            m_moveGun = false;
            m_shootGun = true;
            return;
        }

        m_rectTransform.anchoredPosition = Vector3.Lerp(m_startPosition, m_targetPosition, m_time / m_moveDuration);
    }

    private void Shoot_Gun()
    {
        m_time += Time.deltaTime;
        if (m_shootCount < 2)
        {
            if (m_time >= m_shootDuration)
            {
                m_time = 0f;

                GameObject bulletMark = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/UI/UI_BulletMark"), Vector2.zero, Quaternion.identity, WesternManager.Instance.MainPanel.transform);
                switch (m_shootCount)
                {
                    case 0: // 총자국 생성
                        bulletMark.GetComponent<RectTransform>().anchoredPosition = new Vector3(-438f, 125f, 0f);
                        bulletMark.GetComponent<Image>().sprite = Resources.Load<Sprite>("1. Graphic/3D/2. Western/UI/BulletMark");
                        break;

                    case 1: // 총자국 생성
                        bulletMark.GetComponent<RectTransform>().anchoredPosition = new Vector3(174.9f, -29.4f, 0f);
                        bulletMark.GetComponent<Image>().sprite = Resources.Load<Sprite>("1. Graphic/3D/2. Western/UI/BulletMark");
                        break;
                }
                m_shootCount++;
            }
        }
        else if (m_shootCount == 2) // 암전 및 레벨 전환
        {
            if (m_time >= m_darkDuration)
            {
                UIManager.Instance.Start_FadeWaitAction(1f, Color.black, () => WesternManager.Instance.LevelController.Change_Level((int)WesternManager.LEVELSTATE.LS_PlayLv1), 1f, false);
                m_shootCount++;
            }
        }
        else
        {
            m_shootGun = false;
        }

    }
}
