using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private Coroutine m_moves = null;

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
                    if (m_moves != null)
                        StopCoroutine(m_moves);
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

                case DialogData_Mascot.DIALOGTYPE.DET_WAITGAME:
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

                case DialogData_Mascot.DIALOGTYPE.DET_AYAKA:
                    m_isUpdate = false;
                    Update_Ayaka();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_AYAKADIALOG:
                    Update_AyakaDialog();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_ATTACKAYAKA:
                    if (m_event != null)
                        StopCoroutine(m_event);
                    m_event = StartCoroutine(Wait_AyakaAttack(m_dialogs[m_dialogIndex].animationTriger));
                    Update_None();
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_DESTROYBOX:
                    GameObject canvas = GameObject.Find("Canvas");
                    if (canvas != null)
                    {
                        Transform child = canvas.transform.Find("ayakaBox");
                        if (child != null)
                            GameManager.Ins.Resource.Destroy(child.gameObject);
                        m_dialogIndex++;
                        Update_DialogIndex();
                    }

                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_GOLFSWING:
                    Update_None();
                    if (m_event != null)
                        StopCoroutine(m_event);
                    m_event = StartCoroutine(Wait_GolfSwing(m_dialogs[m_dialogIndex-1].animationTriger));
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_GOLFRESULT:
                    Update_None();
                    if (m_event != null)
                        StopCoroutine(m_event);
                    m_event = StartCoroutine(Wait_GolfResult());
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_MOVES:
                    Update_None();
                    if (m_moves != null)
                        StopCoroutine(m_moves);
                    m_moves = StartCoroutine(Moves_Mascot(m_dialogIndex - 1));
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_MOVESSTOP:
                    Update_None();
                    if (m_moves != null)
                        StopCoroutine(m_moves);
                    break;

                case DialogData_Mascot.DIALOGTYPE.DET_CLICKWESTERN:
                    Update_None();
                    // 서부 클릭 가능
                    GameManager.Ins.Window.FileIconSlots.Set_AllIconClick(WindowManager.FILETYPE.TYPE_WESTERN, true);
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

    private void Update_Ayaka()
    {
        if (m_event != null)
            StopCoroutine(m_event);
        m_event = StartCoroutine(Apear_Ayaka());
    }

    private void Update_AyakaDialog()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform child = canvas.transform.Find("ayakaBox");
            if (child == null)
            {
                GameObject effectPanel = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Mascot/ayaka_DialogBox", canvas.transform);
                effectPanel.name = "ayakaBox";
                child = effectPanel.transform;
            }

            TMP_Text text = child.GetChild(1).GetComponent<TMP_Text>();
            if(text != null)
            {
                if (m_event != null)
                    StopCoroutine(m_event);
                m_event = StartCoroutine(Type_Ayaka(text, m_dialogs[m_dialogIndex].dialogText));
            }

            if (string.IsNullOrEmpty(m_dialogs[m_dialogIndex].animationTriger) == false)
            {
                Transform ayaka = canvas.transform.Find("Ayaka");
                if (ayaka != null)
                    ayaka.GetComponent<Image>().sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/Sprite/Window_SceneEffect_VisualNovel_AYAKA" + m_dialogs[m_dialogIndex].animationTriger);
            }
        }

        m_dialogIndex++;
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

    IEnumerator Moves_Mascot(int index)
    {
        Vector2 startPos = m_rt.anchoredPosition;
        Vector2 targetPos = new Vector2(m_dialogs[index].targetPosition[0], m_dialogs[index].targetPosition[1]);

        float moveSpeed = 50f;
        while (true)
        {
            Vector2 direction = (targetPos - m_rt.anchoredPosition).normalized;
            m_rt.anchoredPosition += direction * moveSpeed * Time.deltaTime;

            if (Vector2.Distance(m_rt.anchoredPosition, targetPos) < 0.1f)
            {
                m_rt.anchoredPosition = targetPos;
                yield break;
            }

            yield return null;
        }
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

    IEnumerator Apear_Ayaka()
    {
        // 미연시 파일 초기화
        GameManager.Ins.Window.FileIconSlots.Remove_FileIcon("C:\\Users\\user\\Desktop\\오싹오싹 밴드부");
        // 아이콘이 찢어짐
        GameObject effectPanel = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Mascot/effectPanel", GameObject.Find("Canvas").transform);
        effectPanel.name = "Ayaka";
        effectPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-389.5f, 287.7f);
        effectPanel.transform.localScale = new Vector3(1.045f, 1.045f, 1.045f);
        Image iconImage = effectPanel.GetComponent<Image>();
        iconImage.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/BrokenIcon/Window_SceneEffect_BrokenIcon");
        yield return new WaitForSeconds(0.2f);
        iconImage.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/BrokenIcon/Window_SceneEffect_BrokenIcon2");
        yield return new WaitForSeconds(0.2f);
        iconImage.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/BrokenIcon/Window_SceneEffect_BrokenIcon3");
        yield return new WaitForSeconds(1f);

        // 스탠딩 이동
        RectTransform standingRect = effectPanel.GetComponent<RectTransform>();
        standingRect.anchoredPosition = new Vector2(-389.5f, 287.7f);
        effectPanel.transform.localScale = new Vector3(0.5634848f, 1.894861f, 1.045f);
        effectPanel.GetComponent<Image>().sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/c4");
        yield return new WaitForSeconds(1f);

        // A위치로 이동
        standingRect.anchoredPosition = new Vector2(-519f, 131f);
        effectPanel.transform.localScale = new Vector3(0.8782474f, 2.95333f, 1.628737f);
        yield return new WaitForSeconds(1f);

        // B위치로 이동
        standingRect.anchoredPosition = new Vector2(-626f, -120f);
        effectPanel.transform.localScale = new Vector3(1.244652f, 4.18546f, 2.308246f);
        yield return new WaitForSeconds(1f);

        // C위치로 이동
        standingRect.anchoredPosition = new Vector2(-367f, -222f);
        effectPanel.transform.localScale = new Vector3(1.702435f, 5.724872f, 3.157219f);
        yield return new WaitForSeconds(1f);

        // D위치로 이동
        standingRect.anchoredPosition = new Vector2(0f, -768f);
        effectPanel.transform.localScale = new Vector3(7.14520597f, 24.0275784f, 13.251008f);

        m_dialogIndex++;
        Update_DialogIndex();
        yield return new WaitForSeconds(1f);

        // E위치로 이동 KI08
        effectPanel.GetComponent<Image>().sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/Sprite/Window_SceneEffect_VisualNovel_AYAKA0");
        standingRect.anchoredPosition = new Vector2(90f, 14.8f);
        standingRect.sizeDelta = new Vector2(1065f, 1068f);
        effectPanel.transform.localScale = new Vector3(0.9613168f, 0.9613168f, 0.9613168f);
        yield return new WaitForSeconds(1f);

        // 다음 다이얼로그 업데이트
        m_isUpdate = true;
        yield break;
    }

    IEnumerator Type_Ayaka(TMP_Text tmpText, string text)
    {
        string dialogText = text.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

        m_isUpdate = false;
        tmpText.text = "";
        foreach (char letter in dialogText.ToCharArray())
        {
            tmpText.text += letter;
            yield return new WaitForSeconds(m_typeSpeed);
        }

        m_isUpdate = true;
        yield break;
    }

    IEnumerator Wait_AyakaAttack(string trigerName)
    {
        m_isUpdate = false;
        while (true)
        {
            if (m_animator.IsInTransition(0) == false)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(trigerName) == true)
                {
                    float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (animTime >= 0.65f) // 애니메이션 종료
                        break;
                }
            }

            yield return null;
        }

        m_isUpdate = true;
        Update_DialogIndex();
        yield break;
    }

    IEnumerator Wait_GolfSwing(string trigerName)
    {
        m_isUpdate = false;
        while (true)
        {
            if (m_animator.IsInTransition(0) == false)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(trigerName) == true)
                {
                    float animTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (animTime >= 0.5f) // 애니메이션 종료
                        break;
                }
            }

            yield return null;
        }

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform ayaka = canvas.transform.Find("Ayaka");
            if (ayaka != null)
                GameManager.Ins.Resource.Destroy(ayaka.gameObject);;

        }

        m_isUpdate = true;
        Update_DialogIndex();
        yield break;
    }

    IEnumerator Wait_GolfResult()
    {
        GameObject effectPanel = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Mascot/effectPanel", GameObject.Find("Canvas").transform);
        
        Image iconImage = effectPanel.GetComponent<Image>();
        iconImage.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/Window_SceneEffect_FlyingBodies");
        
        RectTransform standingRect = effectPanel.GetComponent<RectTransform>();
        Vector2 startPosition = new Vector2(530f, 719f);
        Vector2 endPosition = new Vector2(530f, -100f);
        standingRect.anchoredPosition = startPosition;
        standingRect.sizeDelta = new Vector2(1067f, 2048f);
        effectPanel.transform.localScale = new Vector3(0.066f, 0.066f, 0.066f);

        m_isUpdate = false;

        yield return new WaitForSeconds(2f);

        float duration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            standingRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        standingRect.anchoredPosition = endPosition;

        iconImage.sprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/1. VisualNovel/UI/VisualNovel/Window_SceneEffect_FlyingBodies_Icon");
        standingRect.sizeDelta = new Vector2(930f, 330f);
        effectPanel.transform.localScale = new Vector3(0.16895814f, 0.1689581f, 0.1689581f);

        m_isUpdate = true;
        Update_DialogIndex();
        yield break;
    }
    #endregion

    IEnumerator Type_Text(int dialogIndex)
    {
        m_dialogs[dialogIndex].dialogText = m_dialogs[dialogIndex].dialogText.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

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
