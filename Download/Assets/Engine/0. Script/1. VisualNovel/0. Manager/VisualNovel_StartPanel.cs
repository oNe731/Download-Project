using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovel
{
    public class VisualNovel_StartPanel : MonoBehaviour
    {
        [SerializeField] private VisualNovelManager.LEVELSTATE m_startState;

        public void Button_Start()
        {
            Destroy(gameObject);
            GameManager.Ins.Novel.LevelController.Change_Level((int)m_startState);
        }

        public void Button_Exit()
        {
            GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW);
        }
    }

}