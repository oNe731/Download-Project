using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interaction
{
    public enum OPENTYPE { OT_BASICONE, OT_BASICTWO, OT_ANIMATION, OT_END };
    public enum EVENTTYPE { ET_NONE, ET_CLEAR, ET_EVENT, ET_END };

    [SerializeField] private int m_doorIndex;
    [SerializeField] private OPENTYPE  m_openType;
    [SerializeField] private EVENTTYPE m_eventType;
    [SerializeField] private Vector3 m_openOffset; // y 150
    [SerializeField] private float m_duration = 2f;
    public int DoorIndex => m_doorIndex;

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
        if (m_interactionUI == null || m_interactionUI.gameObject.activeSelf == false || m_interact == true)
            return;

        switch (m_eventType)
        {
            case EVENTTYPE.ET_NONE:
                Open_Door();
                break;

            case EVENTTYPE.ET_CLEAR:
                Check_Clear();
                break;

            case EVENTTYPE.ET_EVENT:
                Open_Door();
                Check_Event();
                break;
        }
    }

    private void Check_Clear()
    {
        // 해당 구역의 특정 조건 성립 시 문열림
        // 아닐 시 문구 출력
        float[] activeTimes = new float[1];
        string[] texts = new string[1];

        LevelController levelController = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Levels;
        if(levelController == null) // 본 스테이지 클리어 여부 판별
            m_interact = HorrorManager.Instance.LevelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref activeTimes, ref texts);
        else                        // 세부 스테이지 클리어 여부 판별
            m_interact = levelController.Get_CurrentLevel<Horror_Base>().Check_Clear(this, ref activeTimes, ref texts);

        if (m_interact == true)
            Open_Door();
        else
            HorrorManager.Instance.Active_InstructionUI(UIInstruction.ACTIVETYPE.TYPE_BASIC, UIInstruction.ACTIVETYPE.TYPE_FADE, activeTimes, texts);
    }

    private void Check_Event()
    {
        // 문 열림
        // 선택적 이벤트 추가 발생 (몬스터 생성 등)

        Debug.Log("문 열고 이벤트 발생 판별");
    }

    public void Open_Door()
    {
        Destroy(m_interactionUI.gameObject);

        switch(m_openType)
        {
            case OPENTYPE.OT_BASICONE:
                StartCoroutine(Open_OneMove());
                break;
            case OPENTYPE.OT_BASICTWO:
                break;
            case OPENTYPE.OT_ANIMATION:
                Open_Animation();
                break;
        }
    }

    private IEnumerator Open_OneMove()
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

    private void Open_Animation()
    {
        // Temp
        Destroy(gameObject);

        // 열리는 문 애니메이션 재생
        //
    }
}
