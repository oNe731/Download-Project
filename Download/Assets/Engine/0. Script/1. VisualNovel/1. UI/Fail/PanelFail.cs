using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFail : MonoBehaviour
{
    private GameObject m_handObject;
    private Image m_image;

    public void Start_PanelFail(GameObject handObject)
    {
        m_image = GetComponent<Image>();

        GameManager.Ins.Set_Pause(true);
        m_handObject = handObject;

        StartCoroutine(Fade_Color(1.0f));
    }

    private IEnumerator Fade_Color(float duration)
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        GameManager.Ins.Mascot.Start_Dialog("4. Data/Mascot/VisualNovel/Mascot_ChaseFail", true);

        Color startColor = m_image.color;
        Color targetColor = new Color(0f, 0f, 0f, startColor.a);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            m_image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        m_image.color = targetColor;
        yield break;
    }

    public void Panel_Return()
    {
        // 되감기 연출 재생
        StartCoroutine(Update_Image(5f));
    }
    private IEnumerator Update_Image(float duration)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        // 스프라이트 배열 불러오기
        int currentIndex = 0;
        Sprite[] m_sprite = GameManager.Ins.Resource.LoadAll<Sprite>("1. Graphic/2D/1. VisualNovel/UI/RetryImage/");
        m_image.sprite = m_sprite[currentIndex];
        m_image.color = Color.white;

        float Totaltime = 0;
        float Changetime = 0;

        while (Totaltime < duration)
        {
            Totaltime += Time.deltaTime;
            Changetime += Time.deltaTime;
            if(Changetime > 0.04f)
            {
                Changetime = 0f;
                currentIndex++;
                if(currentIndex >= m_sprite.Length)
                    currentIndex = 0;
                m_image.sprite = m_sprite[currentIndex];
            }

            yield return null;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        m_image.color = Color.black;

        // 완료 시 다이얼로그 업데이트
        GameManager.Ins.Mascot.IsUpdate = true;
        yield break;
    }

    public void Button_Yes()
    {
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Yes_Update(), 1f, false);
    }

    private void Yes_Update()
    {
        // 손, 패널 삭제
        Destroy(m_handObject);
        Destroy(gameObject);

        // 추격 게임방법 설명 UI에서 다시 시작
        GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_DAY3CHASEGAME, true);
    }
}
