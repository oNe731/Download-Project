using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interaction
{
    public enum EVENTTYPE { ET_CLEAR, ET_EVENT, ET_END };

    [SerializeField] private EVENTTYPE m_eventType;
    [SerializeField] private Vector3 m_openOffset; // y 150
    [SerializeField] private float m_duration = 2f;

    private void Start()
    {
        GameObject gameObject = HorrorManager.Instance.Create_WorldHintUI(UIWorldHint.HINTTYPE.HT_OPENDOOR, transform.GetChild(0), m_uiOffset);
        m_interactionUI = gameObject.GetComponent<UIWorldHint>();
    }

    private void Update()
    {
        //Update_InteractionUI();
    }

    public override void Click_Interaction()
    {
        if (m_interactionUI.gameObject.activeSelf == false || m_interact == true)
            return;

        switch (m_eventType)
        {
            case EVENTTYPE.ET_CLEAR:
                Check_Clear();
                break;

            case EVENTTYPE.ET_EVENT:
                Check_Event();
                break;
        }
    }

    private void Check_Clear()
    {
        // 해당 구역의 특정 조건 성립 시 문열림
        // 아닐 시 문구 출력
        string text = "";

        LevelController levelController = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Levels;
        if(levelController == null) // 본 스테이지 클리어 여부 판별
            m_interact = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Check_Clear(ref text);
        else                        // 세부 스테이지 클리어 여부 판별
            m_interact = levelController.Get_CurrentLevel<Horror_Base>().Check_Clear(ref text);

        if (m_interact == true)
            Open_Door();
        else
            Print_Text(text);
    }

    private void Check_Event()
    {
        // 문 열림
        // 선택적 이벤트 추가 발생

        Debug.Log("문 열고 이벤트 발생 판별");
    }

    private void Open_Door()
    {
        Destroy(m_interactionUI.gameObject);
        StartCoroutine(Open_Move());
    }

    IEnumerator Open_Move()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation   = startRotation * Quaternion.Euler(m_openOffset.x, m_openOffset.y, m_openOffset.z);
        
        float elapsedTime = 0;
        while (elapsedTime < m_duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / m_duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.rotation = endRotation;
    }

    private void Print_Text(string text)
    {
        // < 이 스크립트는 1.5초동안 유지된다. (페이드인X, 페이드 아웃 O)
        // 해당 스크립트가 아직 사라지지 않았다면 다시 좌클릭을 해도 추가로 스크립트가 뜨지 않는다.
        GameObject ui = GameManager.Instance.Create_GameObject("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.GetChild(2));
        if (ui == null)
            return;
        UIPopup.Expendables info = new UIPopup.Expendables();
        info.text = text;
        ui.GetComponent<UIPopup>().Initialize_UI(UIPopup.TYPE.T_EXPENDABLES, info);
    }
}
