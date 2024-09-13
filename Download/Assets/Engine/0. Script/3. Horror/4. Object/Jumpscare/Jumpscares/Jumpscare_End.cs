using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare_End : Jumpscare // 임시 클리어 작업
{
    public override void Active_Jumpscare()
    {
        m_isTrigger = true;

        HorrorManager.Instance.Set_Pause(true); // 게임 일시정지
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/Canvas_GameClear");
        if (gameObject == null)
            return;
        StartCoroutine(End_Game());
    }

    IEnumerator End_Game()
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
