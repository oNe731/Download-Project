using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WesternManager : MonoBehaviour
{
    private static WesternManager m_instance = null;
    public static WesternManager Instance
    {
        get
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    // 게임 로고 -> 인트로컷씬(클릭/스페이스/엔터로 넘김) -> 수배지확인화면 -> 게임 시작
    public enum LEVELSTATE { LS_LEVEL1, LS_LEVEL2, LS_LEVEL3, LS_END };
    [SerializeField] GameObject m_StartPanel;
    [SerializeField] private LEVELSTATE m_StartState = LEVELSTATE.LS_END;
    private LEVELSTATE m_LevelState = LEVELSTATE.LS_END;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void Button_Start()
    {
        Destroy(m_StartPanel);
        //Change_Level(m_StartState);
    }

    public void Button_Exit()
    {
        SceneManager.LoadScene("Window");
    }
}
