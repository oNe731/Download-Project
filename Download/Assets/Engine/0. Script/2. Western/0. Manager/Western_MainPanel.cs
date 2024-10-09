using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Western_MainPanel : MonoBehaviour
{
    public void Button_Play(GameObject gameObject)
    {
        if(GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western.Western_Main>().Button_Play() == true)
        {
            if (gameObject != null)
                gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
