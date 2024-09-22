using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide_Clear : Collide
{
    public override void Trigger_Event()
    {
        HorrorManager.Instance.Set_Pause(true); // 게임 일시정지
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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        yield break;
    }
}
