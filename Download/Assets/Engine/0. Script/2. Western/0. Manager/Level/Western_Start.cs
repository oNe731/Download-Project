using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Western
{
    public class Western_Start : MonoBehaviour
    {
        public enum Type { Type_Start, Type_Exit, Type_End }

        [SerializeField] private WesternManager.LEVELSTATE m_startState = WesternManager.LEVELSTATE.LS_IntroLv1;
        [SerializeField] private GameObject m_arrow;
        private Type m_currentType = Type.Type_Start;

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                switch (m_currentType)
                {
                    case Type.Type_Start:
                        Button_Start();
                        break;

                    case Type.Type_Exit:
                        Button_Exit();
                        break;
                }
            }
            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                Update_Input();
        }

        public void Button_Start()
        {
            Update_Index(Type.Type_Start);
        }

        public void Button_Exit()
        {
            Update_Index(Type.Type_Exit);
        }

        public void Update_Index(Type type)
        {
            switch (type)
            {
                case Type.Type_Start:
                    if (m_currentType == type)
                    {
                        Destroy(gameObject);
                        WesternManager.Instance.LevelController.Change_Level((int)m_startState);
                        break;
                    }
                    Update_Position(Type.Type_Start);
                    m_currentType = type;
                    break;

                case Type.Type_Exit:
                    if (m_currentType == type)
                    {
                        SceneManager.LoadScene("Window");
                        break;
                    }
                    Update_Position(Type.Type_Exit);
                    m_currentType = type;
                    break;
            }
        }

        public void Update_Position(Type type)
        {
            switch (type)
            {
                case Type.Type_Start:
                    m_arrow.GetComponent<RectTransform>().anchoredPosition = new Vector3(-114.6f, -332f, 0f);
                    break;

                case Type.Type_Exit:
                    m_arrow.GetComponent<RectTransform>().anchoredPosition = new Vector3(-94.0f, -428f, 0f);
                    break;
            }
        }

        public void Update_Input()
        {
            if (m_currentType == Type.Type_Start)
                m_currentType = Type.Type_Exit;
            else if (m_currentType == Type.Type_Exit)
                m_currentType = Type.Type_Start;
            Update_Position(m_currentType);
        }
    }
}