using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide_Clear : Collide
{
    public override void Trigger_Event()
    {
        GameManager.Ins.Set_Pause(true); // 게임 일시정지
        GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/Canvas_GameClear");

        StartCoroutine(End_Game());
    }

    private IEnumerator End_Game()
    {
        float time = 0;
        while(time < 2f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        GameManager.Ins.End_Game();
        yield break;
    }
}
