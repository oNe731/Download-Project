using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Western_MainPanel : MonoBehaviour
{
    public void Button_Play()
    {
        GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western.Western_Main>().Button_Play();
    }
}
