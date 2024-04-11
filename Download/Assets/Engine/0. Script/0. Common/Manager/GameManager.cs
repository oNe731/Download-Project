using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance
    {
        get //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 호출 가능
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }


    private string m_playerName = null;
    public string PlayerName
    {
        get 
        { 
            return m_playerName;
        }
        set
        {
            if(value.Length > 0)
                m_playerName = value;
        }
    }

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            //씬 전환이 되더라도 파괴되지 않음
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //이미 전역변수인 instance에 인스턴스가 존재한다면 자신을 삭제
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
