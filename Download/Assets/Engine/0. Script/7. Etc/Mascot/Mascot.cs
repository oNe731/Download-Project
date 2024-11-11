using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mascot : MonoBehaviour
{
    [SerializeField] private TMP_Text m_dialogTxt;
    [SerializeField] private Animator m_animator;

    private bool m_isUpdate = false;
    private bool m_isfinishClose = false;
    private float m_time = 0;
    private GameObject m_balloon;
    private RectTransform m_rt = null;

    private Coroutine m_event = null;

    private List<DialogData_Mascot> m_dialogs;
    private Coroutine m_dialogTextCoroutine = null;
    private bool m_isTyping = false;
    private int m_dialogIndex = 0;
    private float m_typeSpeed = 0.05f;

    private float m_waitTime = 2f;

    public bool IsUpdate { get => m_isUpdate; set => m_isUpdate = value; }
    public RectTransform Rt => m_rt;

    public void Initialize_Mascot()
    {
        m_balloon = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        m_rt = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }

    public void Start_Dialog(string path, bool finishClose)
    {
        m_dialogs = GameManager.Ins.Load_JsonData<DialogData_Mascot>(path);
        m_isfinishClose = finishClose;

        m_isUpdate = true;
        m_isTyping = false;
        m_dialogIndex = 0;

        gameObject.SetActive(true);
        Update_DialogIndex();

    }

    private void Update()
    {
        //if (GameManager.Ins.IsGame == true)
        //    return;

        if (m_isUpdate == false || m_isTyping == true)
            return;

        m_time += Time.deltaTime;
        if(m_time > m_waitTime)
        {
            Update_DialogIndex();
        }
    }

    private void Update_DialogIndex()
    {
        m_time = 0;

        // 다이얼로그 진행
        if (m_dialogIndex < m_dialogs.Count)
        {
            switch (m_dialogs[m_dialogIndex].dialogEvent)
            {
                case DialogData_Mascot.DIALOGTYPE.DET_NONE:
                    Update_None();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_ANIMWAIT:
                    Update_WaitAnim();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_MOVE:
                    Update_Move();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_DELETGAME:
                    Update_DeleteGame();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_DELETFINISH:
                    Update_DeleteFinish();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_CREATEGAME:
                    Update_CreateGame();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_WAITNOVEL:
                    Update_WaitNovel();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_CLICKNOVEL:
                    Update_ClickNovel();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_NOVELEXIT:
                    Update_NovelExit();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_NOVELRETURN:
                    m_isUpdate = false;
                    // 기본 다이얼로그 업데이트 
                    Update_None();
                    // 다시 감기 연출 재생
                    VisualNovel.Novel_Day3Chase level = GameManager.Ins.Novel.LevelController.Get_CurrentLevel<VisualNovel.Novel_Day3Chase>();
                    if(level != null)
                    {
                        if(level.FailPanel != null)
                            level.FailPanel.Panel_Return();
                    }
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_NOVELRESTART:
                    // 재시작
                    VisualNovel.Novel_Day3Chase levels = GameManager.Ins.Novel.LevelController.Get_CurrentLevel<VisualNovel.Novel_Day3Chase>();
                    if (levels != null)
                    {
                        if (levels.FailPanel != null)
                            levels.FailPanel.Button_Yes();
                    }

                    // 다이얼로그 종료
                    gameObject.SetActive(false);
                    break;
            }
        }
        else // 다이얼로그 종료
        {
            if(m_isfinishClose == true)
                gameObject.SetActive(false);
        }
    }

    #region Update
    private void Update_None()
    {
        // 위치값 사용 시
        if (m_dialogs[m_dialogIndex].setPosition == true)
            m_rt.anchoredPosition = new Vector2(m_dialogs[m_dialogIndex].startPosition[0], m_dialogs[m_dialogIndex].startPosition[1]);

        // 다이얼로그 내용 업데이트
        if (string.IsNullOrEmpty(m_dialogs[m_dialogIndex].dialogText) == false)
        {
            m_balloon.SetActive(true);
            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex));
        }
        else
        {
            m_balloon.SetActive(false);
        }

        // 애니메이션 사용 시
        Play_Animation();

        // 이동 사용 시
        if (m_dialogs[m_dialogIndex].movePosition == true)
        {

        }

        m_dialogIndex++;
    }

    private void Play_Animation()
    {
        if (string.IsNullOrEmpty(m_dialogs[m_dialogIndex].animationTriger) == false)
            m_animator.SetTrigger(m_dialogs[m_dialogIndex].animationTriger);
    }

    private void Update_WaitAnim()
    {
        Update_None();

        if (m_event != null)
            StopCoroutine(m_event);
        m_event = StartCoroutine(Wait_Anim(m_dialogs[m_dialogIndex - 1].animationTriger));
    }

    private void Update_Move()
    {
        Update_None();

        if (m_event != null)
            StopCoroutine(m_event);
        m_event = StartCoroutine(Move_Mascot(m_dialogIndex - 1));
    }

    private void Update_DeleteGame()
    {
        Play_Animation();
        GameManager.Ins.StartCoroutine(GameManager.Ins.Window.Folder.Destroy_GameIcon());

        m_dialogIndex++;
        Update_DialogIndex();
    }

    private void Update_DeleteFinish() // 삭제 완료시까지 대기 후 진행
    {
        if (m_event != null)
            StopCoroutine(m_event);
        m_event = StartCoroutine(Wait_Delet());
    }

    private void Update_CreateGame()
    {
        Update_None();

        if (m_event != null)
            StopCoroutine(m_event);
        m_event = StartCoroutine(Wait_Crate(m_dialogs[m_dialogIndex - 1].animationTriger));
    }

    private void Update_WaitNovel()
    {
        if (m_event != null)
            StopCoroutine(m_event);
        m_event = StartCoroutine(Wait_Novel());
    }

    private void Update_ClickNovel()
    {
        Update_None();

        // 미연시 클릭 가능
        GameManager.Ins.Window.FileIconSlots.Set_AllIconClick(WindowManager.FILETYPE.TYPE_NOVEL, true);
    }

    private void Update_NovelExit()
    {
        VisualNovel.Novel_Day3Chase level =GameManager.Ins.Novel.LevelController.Get_CurrentLevel<VisualNovel.Novel_Day3Chase>();
        if (level == null)
            return;

        // 비상구 활성화
        level.Exit.gameObject.SetActive(true);

        // 다이얼로그 업데이트
        Update_None();
    }

    IEnumerator Wait_Anim(string trigerName)
    {
        m_isUpdate = false;
        while (true)
        {
            if (m_animator.IsInTransition(0) == false)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(trigerName) == true)
                {
                    float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (animTime >= 1.0f) // 애니메이션 종료
                        break;
                }
            }

            yield return null;
        }
        m_isUpdate = true;
        Update_DialogIndex();

        yield break;
    }

    IEnumerator Move_Mascot(int index)
    {
        float duration = 1f;
        m_isUpdate = false;

        float m_time = 0;

        Vector2 startPos = m_rt.anchoredPosition;
        Vector2 targetPos = new Vector2(m_dialogs[index].targetPosition[0], m_dialogs[index].targetPosition[1]);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (m_isTyping == false)
                m_time += Time.deltaTime;

            m_rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_rt.anchoredPosition = targetPos;

        while(true)
        {
            if (m_isTyping == false)
            {
                m_time += Time.deltaTime;
                if (m_time > m_waitTime)
                {
                    break;
                }
            }

            yield return null;
        }

        m_isUpdate = true;
        Update_DialogIndex();

        yield break;
    }

    IEnumerator Wait_Delet()
    {
        m_isUpdate = false;

        Panel_Folder folder = GameManager.Ins.Window.Folder;
        while (true)
        {
            if (folder.IsEvent == false)
                break;

            yield return null;
        }

        m_isUpdate = true;
        m_dialogIndex++;
        Update_DialogIndex();

        yield break;
    }

    IEnumerator Wait_Crate(string trigerName)
    {
        m_isUpdate = false;
        while (true)
        {
            if (m_animator.IsInTransition(0) == false)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(trigerName) == true)
                {
                    float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (animTime >= 0.6f)// 애니메이션 중반
                        break;
                }
            }

            yield return null;
        }

        m_isUpdate = true;
        GameManager.Ins.Window.Folder.InputPopupButton = true;
        GameManager.Ins.Window.Folder.Active_Popup(false);

        GameManager.Ins.Window.FileIconSlots.Add_FileIcon(1, 3, WindowManager.FILETYPE.TYPE_NOVEL, "오싹오싹 밴드부", () => GameManager.Ins.Window.WindowButton.Button_VisualNovel());
        GameManager.Ins.Window.FileIconSlots.Add_FileIcon(3, 7, WindowManager.FILETYPE.TYPE_WESTERN, "THE LEGEND COWBOY", () => GameManager.Ins.Window.WindowButton.Button_Western());
        GameManager.Ins.Window.FileIconSlots.Add_FileIcon(2, 10, WindowManager.FILETYPE.TYPE_HORROR, "THE HOSPITAL", () => GameManager.Ins.Window.WindowButton.Button_Horror());

        yield break;
    }

    IEnumerator Wait_Novel()
    {
        m_isUpdate = false;

        float time = 0;
        while (time < 5f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        m_isUpdate = true;
        m_dialogIndex++;
        Update_DialogIndex();

        yield break;
    }
    #endregion

    IEnumerator Type_Text(int dialogIndex)
    {
        m_isTyping = true;

        m_dialogTxt.text = "";
        foreach (char letter in m_dialogs[dialogIndex].dialogText.ToCharArray())
        {
            m_dialogTxt.text += letter;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isTyping = false;
        yield break;
    }
}
