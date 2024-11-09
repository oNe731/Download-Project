using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Western
{
    public class Dialog_PlayWT : Dialog<DialogData_PlayWT>
    {
        [Header("GameObject")]
        [SerializeField] private GameObject m_dialogTxtObj;
        [SerializeField] private GameObject m_backgroundObj;
        [SerializeField] private GameObject m_profileObj;

        private bool m_active = false;

        private TMP_Text m_dialogTxt;
        private RectTransform m_dialogTxtrectTransform;
        private RectTransform m_backgroundrectTransform;
        private Image m_profileImg;

        private float m_activeTime = 0f;
        private float m_duration = 0.5f;
        private Vector3 m_startPosition;
        private Vector3 m_targetPosition;

        private float m_shakeTime = 0.3f;
        private float m_shakeAmount = 5.0f; // 세기

        public bool Active => m_active;
        public bool LastIndex
        {
            get
            {
                if (m_dialogIndex == m_dialogs.Count && m_isTyping == false)
                    return true;
                else
                    return false;
            }
        }

        private void Awake()
        {
            m_dialogTxt = m_dialogTxtObj.GetComponent<TMP_Text>();
            m_dialogTxtrectTransform = m_dialogTxtObj.GetComponent<RectTransform>();
            m_backgroundrectTransform = m_backgroundObj.GetComponent<RectTransform>();
            m_profileImg = m_profileObj.GetComponent<Image>();

            m_startPosition = new Vector3(48f, 0f, 0f);
            m_targetPosition = new Vector3(48f, 300f, 0f);
        }

        private void Update_Dialog()
        {
            if (m_isTyping)
                m_cancelTyping = true;
            else if (!m_isTyping)
            {
                // 다이얼로그 진행
                if (m_dialogIndex < m_dialogs.Count)
                {
                    switch (m_dialogs[m_dialogIndex].dialogEvent)
                    {
                        case DialogData_PlayWT.DIALOGEVENT_TYPE.DET_NONE:
                            Update_None();
                            break;

                        case DialogData_PlayWT.DIALOGEVENT_TYPE.DET_FADEIN:
                            Update_FadeIn();
                            break;

                        case DialogData_PlayWT.DIALOGEVENT_TYPE.DET_FADEOUT:
                            Update_FadeOut();
                            break;

                        case DialogData_PlayWT.DIALOGEVENT_TYPE.DET_FADEOUTIN:
                            Update_FadeOutIn();
                            break;

                        case DialogData_PlayWT.DIALOGEVENT_TYPE.DET_WAIT:
                            Update_None();
                            if (m_dialogIndex == m_dialogs.Count)
                                StartCoroutine(Update_WaitClose());
                            else
                                StartCoroutine(Update_WaitUpdate());
                            break;

                        case DialogData_PlayWT.DIALOGEVENT_TYPE.DET_BOMB:
                            Update_Bomb();
                            break;

                        case DialogData_PlayWT.DIALOGEVENT_TYPE.DET_TUTORIAL:
                            Western_PlayLv1 level = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
                            if (level != null)
                            {
                                level.Change_FinishDialog();
                                StartCoroutine(Update_WaitClose());
                            }
                            break;
                    }
                }
                else // 다이얼로그 종료
                {
                    Close_Dialog();
                }
            }
        }

        #region Update
        private void Update_Basic(int index)
        {
            // 리소스 업데이트
            if (m_dialogs[index].fontColor.Length != 0)
                m_dialogTxt.color = new Color(m_dialogs[index].fontColor[0], m_dialogs[index].fontColor[1], m_dialogs[index].fontColor[2], m_dialogs[index].fontColor[3]);
            if (!string.IsNullOrEmpty(m_dialogs[index].profileSpr))
                m_profileImg.sprite = GameManager.Ins.Western.BackgroundSpr[m_dialogs[index].profileSpr];
        }

        private void Update_None()
        {
            Update_Basic(m_dialogIndex);

            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex, m_dialogTxt));
            m_dialogIndex++;
        }

        private void Update_FadeIn()
        {
            Update_Basic(m_dialogIndex + 1);

            m_dialogTxt.text = "";
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Next_FadeIn());
        }

        private void Next_FadeIn()
        {
            m_dialogIndex++;
            Update_Dialog();
        }

        private void Update_FadeOut()
        {
            if (!string.IsNullOrEmpty(m_dialogs[m_dialogIndex].eventInfo))
            {
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black,
                    () => Start_Dialog(false, GameManager.Ins.Load_JsonData<DialogData_PlayWT>(m_dialogs[m_dialogIndex].eventInfo)), 0f, false);
            }
            else
            {
                // 비어있을 시 페이드 아웃만 진행
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black);
            }

        }

        private void Update_FadeOutIn()
        {
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Update_FadeIn(), 0.5f, false);
        }

        private void Update_Bomb()
        {
            // 폭탄 생성
            Western_PlayLv1 play = GameManager.Ins.Western.LevelController.Get_CurrentLevel<Western_PlayLv1>();
            if (play == null)
                return;
            Transform groupTr = play.Groups.Get_CurrentGroup().transform;
            if (groupTr == null)
                return;

            GameObject Bomb = GameManager.Ins.Resource.LoadCreate("5. Prefab/2. Western/Common/Bomb", Vector3.zero, Quaternion.identity);
            int dir = Random.Range(0, 2); // 0, 1
            if (dir == 0) // 왼쪽에 생성
                Bomb.transform.localPosition = groupTr.position + new Vector3(-3f, 0.8f, -0.1f);
            else if (dir == 1) // 오른쪽에 생성
                Bomb.transform.localPosition = groupTr.position + new Vector3(3f, 0.8f, -0.1f);

            Bomb script = Bomb.GetComponent<Bomb>();
            script.BombType = Western.Bomb.TYPE.TP_TUTORIAL;
            script.TargetPosition = groupTr.position;
            script.TimerMax = 4f;

            // 다이얼로그 업데이트
            m_dialogIndex++;
            Update_Dialog();
        }

        IEnumerator Update_WaitClose()
        {
            while (m_activeTime < float.Parse(m_dialogs[m_dialogIndex - 1].eventInfo))
            {
                if (m_active == false)
                    yield break;

                if (m_isTyping == false)
                    m_activeTime += Time.deltaTime;
                yield return null;
            }

            // 게임 일시정지 대기
            while (true)
            {
                if (GameManager.Ins.IsGame == true)
                    break;
                yield return null;
            }

            m_activeTime = 0f;
            Close_Type();
            yield break;
        }

        IEnumerator Update_WaitUpdate()
        {
            while (m_activeTime < float.Parse(m_dialogs[m_dialogIndex - 1].eventInfo))
            {
                if (m_isTyping == false)
                    m_activeTime += Time.deltaTime;
                yield return null;
            }

            // 게임 일시정지 대기
            while(true)
            {
                if (GameManager.Ins.IsGame == true)
                    break;
                yield return null;
            }

            m_activeTime = 0f;
            Update_Dialog();
            yield break;
        }
        #endregion

        #region Each
        public void Close_Type()
        {
            switch (m_dialogs[m_dialogIndex - 1].closeType)
            {
                case DialogData_PlayWT.DIALOGCLOSE_TYPE.DCT_NONE:
                    Close_Dialog();
                    break;

                case DialogData_PlayWT.DIALOGCLOSE_TYPE.DET_MOVE:
                    Close_MoveDialog();
                    break;
            }
        }

        private void Close_MoveDialog()
        {
            StartCoroutine(Dialog_Move(m_startPosition, m_targetPosition, m_duration, false));
        }

        IEnumerator Dialog_Move(Vector3 startPosition, Vector3 targetPosition, float duration, bool active)
        {
            float time = 0f;
            while (time < m_duration)
            {
                m_backgroundrectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            m_backgroundrectTransform.anchoredPosition = targetPosition;

            m_active = active;
            if(m_active == true)
                Update_Dialog();

            yield break;
        }

        IEnumerator Shake_Dialog()
        {
            float timer = 0;
            Vector2 startPosition = m_startPosition;// new Vector3(21.81082f, 4.509644f);

            while (timer < m_shakeTime)
            {
                Vector2 randomPoint = Random.insideUnitCircle * m_shakeAmount;
                m_backgroundrectTransform.anchoredPosition = startPosition + randomPoint; //Vector2.Lerp(startPosition, randomPoint, timer / m_shakeTime);
                timer += Time.deltaTime;
                yield return null;
            }

            m_backgroundrectTransform.anchoredPosition = startPosition;
            yield break;
        }
        #endregion

        #region Common
        public void Start_Dialog(bool openMove, List<DialogData_PlayWT> dialogs = null)
        {
            m_active = true;

            m_dialogs.Clear();
            m_dialogTxt.text = "";
            m_dialogs = dialogs;

            m_isTyping = false;
            m_cancelTyping = false;
            m_dialogIndex = 0;

            m_backgroundrectTransform.anchoredPosition = m_targetPosition;//m_startPosition;
            m_backgroundObj.SetActive(true);

            if(openMove == true)
                StartCoroutine(Dialog_Move(m_targetPosition, m_startPosition, m_duration, true));
            else
                Update_Dialog();
        }

        private void Close_Dialog()
        {
            m_backgroundObj.SetActive(false);
            m_active = false;
        }

        IEnumerator Type_Text(int dialogIndex, TMP_Text currentText)
        {
            Coroutine shackingcoroutine = null;

            if (m_dialogs[dialogIndex].dialogTalk == DialogData_PlayWT.DIALOGTALK_TYPE.DTT_SHACK)
                shackingcoroutine = StartCoroutine(Shake_Dialog());

            m_isTyping = true;
            m_cancelTyping = false;

            currentText.text = "";
            foreach (char letter in m_dialogs[dialogIndex].dialogText.ToCharArray())
            {
                if (m_cancelTyping)
                {
                    currentText.text = m_dialogs[dialogIndex].dialogText;
                    yield break;
                }

                currentText.text += letter;
                yield return new WaitForSeconds(m_typeSpeed);
            }

            m_isTyping = false;

            if (shackingcoroutine != null)
                StopCoroutine(shackingcoroutine);

            yield break;
        }
        #endregion
    }
}


