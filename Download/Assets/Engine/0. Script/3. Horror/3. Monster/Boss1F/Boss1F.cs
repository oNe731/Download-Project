using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1F : Monster
{
    public enum State { ST_APPEAR, ST_WAIT, ST_IDLE, ST_WEAKNESS, ST_RECALL, ST_TENTACLE, ST_TENTACLES, ST_SPHERE, ST_HIT, ST_DIE, ST_END }

    private int m_pattern = 1;
    private float m_cumulativeDamage = 0f;
    private float m_cumulativeMaxDamage = 10f;
    private float m_rotationSpeed = 3f;

    private GameObject m_hpPanel;
    private Slider m_hpslider;

    public int Pattern { get => m_pattern; }
    public float CumulativeDamage { get => m_cumulativeDamage; set => m_cumulativeDamage = value; }
    public float CumulativeMaxDamage { get => m_cumulativeMaxDamage; }
    public float RotationSpeed { get => m_rotationSpeed; set => m_rotationSpeed = value; }

    public GameObject HpPanel => m_hpPanel;

    public override bool Damage_Monster(float damage)
    {
        if (m_isInvincible == true)
            return false;

        m_cumulativeDamage += damage;

        if(base.Damage_Monster(damage) == false)
            m_stateMachine.Change_State((int)State.ST_HIT);

        if(m_hpslider != null)
            m_hpslider.value = m_hp;
        Check_Pattern();

        return true;
    }

    private void Check_Pattern()
    {
        if (m_hp >= m_hpMax * 0.3f)
            m_pattern = 1;
        else
            m_pattern = 2;
    }

    private void Start()
    {
        m_hpMax = 50f;
        m_hp = m_hpMax;
        Create_HpBar();

        m_attack = 1f;
        m_DieStateIndex = (int)State.ST_DIE;
        m_effectOffset = new Vector3(0f, -1f, 0f);

        m_animator = transform.GetChild(0).GetComponent<Animator>();

        m_stateMachine = new StateMachine<Monster>(gameObject);

        List<State<Monster>> states = new List<State<Monster>>();
        states.Add(new Boss1F_Appear(m_stateMachine));    // 0
        states.Add(new Boss1F_Wait(m_stateMachine));      // 1
        states.Add(new Boss1F_Idle(m_stateMachine));      // 2
        states.Add(new Boss1F_Weakness(m_stateMachine));  // 3
        states.Add(new Boss1F_Recall(m_stateMachine));    // 4
        states.Add(new Boss1F_Tentacle(m_stateMachine));  // 5
        states.Add(new Boss1F_Tentacles(m_stateMachine)); // 6
        states.Add(new Boss1F_Sphere(m_stateMachine));    // 7
        states.Add(new Boss1F_Hit(m_stateMachine));       // 8
        states.Add(new Boss1F_Die(m_stateMachine));       // 9

        m_stateMachine.Initialize_State(states, (int)State.ST_APPEAR);
    }

    private void Create_HpBar()
    {
        m_hpPanel = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_1FBossHp", GameObject.Find("Canvas").transform.GetChild(1));
        if (m_hpPanel == null)
            return;

        for (int i = 1; i < m_hpPanel.transform.childCount; ++i)
        {
            Slider uiSlider = m_hpPanel.transform.GetChild(i).GetComponent<Slider>();
            if (uiSlider != null)
            {
                uiSlider.maxValue = m_hpMax;
                uiSlider.value = m_hp;
            }
        }

        m_hpslider = m_hpPanel.transform.GetChild(2).GetComponent<Slider>();
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        if (m_stateMachine == null)
            return;

        m_stateMachine.Update_State();
    }

    private void OnDestroy()
    {
        GameManager.Ins.Resource.Destroy(m_hpPanel);
    }

    private void OnDrawGizmos()
    {
        if (m_stateMachine == null)
            return;

        m_stateMachine.OnDrawGizmos();
    }
}
