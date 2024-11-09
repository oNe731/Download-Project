using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Western
{
    public class Dialog_IntroWT : Dialog<DialogData_IntroWT>
    {
        [Header("GameObject")]
        [SerializeField] private TMP_Text m_dialogTxt;
        [SerializeField] private GameObject m_arrowObj;
        [SerializeField] private GameObject m_backgroundObj;

        private bool m_isUpdate = true;
        private Coroutine m_arrowCoroutine = null;
        private Coroutine m_eventCoroutine = null;
        private Image m_backgroundImg;

        private void Awake()
        {
            m_arrowSpeed = 0.6f;
            m_backgroundImg = m_backgroundObj.GetComponent<Image>();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (GameManager.Ins.IsGame == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                Update_Dialog();
        }

        private void Update_Dialog()
        {
            if (m_isTyping)
                m_cancelTyping = true;
            else if (!m_isTyping)
            {
                if (m_isUpdate == false)
                    return;

                // 다이얼로그 진행
                if (m_dialogIndex < m_dialogs.Count)
                {
                    switch (m_dialogs[m_dialogIndex].dialogEvent)
                    {
                        case DialogData_IntroWT.DIALOGEVENT_TYPE.DET_NONE:
                            Update_None();
                            break;

                        case DialogData_IntroWT.DIALOGEVENT_TYPE.DET_FADEIN:
                            Update_FadeIn();
                            break;

                        case DialogData_IntroWT.DIALOGEVENT_TYPE.DET_FADEOUT:
                            Update_FadeOut();
                            break;

                        case DialogData_IntroWT.DIALOGEVENT_TYPE.DET_FADEOUTIN:
                            Update_FadeOutIn();
                            break;

                        case DialogData_IntroWT.DIALOGEVENT_TYPE.DET_NEXTMAIN:
                            Update_NextMain();
                            break;

                        case DialogData_IntroWT.DIALOGEVENT_TYPE.DET_STAGE2:
                            Update_Stage2();
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
            m_arrowObj.SetActive(false);

            // 리소스 업데이트
            if (m_dialogs[index].fontColor.Length != 0)
                m_dialogTxt.color = new Color(m_dialogs[index].fontColor[0], m_dialogs[index].fontColor[1], m_dialogs[index].fontColor[2], m_dialogs[index].fontColor[3]);
            if (!string.IsNullOrEmpty(m_dialogs[index].backgroundSpr))
                m_backgroundImg.sprite = GameManager.Ins.Western.BackgroundSpr[m_dialogs[index].backgroundSpr];
        }

        private void Update_None()
        {
            Update_Basic(m_dialogIndex);

            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex, m_dialogTxt, m_arrowObj));
            m_dialogIndex++;
        }

        private void Update_FadeIn()
        {
            Update_Basic(m_dialogIndex + 1);

            m_dialogTxt.text = "";
            m_isUpdate = false;
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Next_FadeIn());
        }

        private void Next_FadeIn()
        {
            m_isUpdate = true;
            m_dialogIndex++;
            Update_Dialog();
        }

        private void Update_FadeOut()
        {
            m_isUpdate = false;
            if (!string.IsNullOrEmpty(m_dialogs[m_dialogIndex].nextInfo))
            {
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black,
                    () => Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_IntroWT>(m_dialogs[m_dialogIndex].nextInfo)), 0f, false);
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

        private void Update_NextMain()
        {
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Next_NextMain(), 0.5f, false);
        }

        private void Next_NextMain()
        {
            Close_Dialog();
            GameManager.Ins.Western.LevelController.Change_Level(int.Parse(m_dialogs[m_dialogIndex].nextInfo));

            m_dialogIndex++;
        }

        private void Update_Stage2()
        {
            Update_None();
            m_eventCoroutine = StartCoroutine(Start_Stage2());

            m_isUpdate = false;
        }

        IEnumerator Start_Stage2()
        {
            float time = 0f;
            while (true)
            {
                if(false == m_isTyping)
                {
                    time += Time.deltaTime;
                    if(time > 1f)
                    {
                        GameManager.Ins.UI.Start_FadeWaitAction(1f, Color.black, () => GameManager.Ins.Western.LevelController.Change_Level((int)WesternManager.LEVELSTATE.LS_PlayLv2), 2f, false);
                        break;
                    }
                }

                yield return null;
            }

            yield break;
        }
        #endregion

        #region Each
        #endregion

        #region Common
        public void Start_Dialog(List<DialogData_IntroWT> dialogs = null)
        {
            m_dialogs = dialogs;

            m_isUpdate = true;
            m_isTyping = false;
            m_cancelTyping = false;
            m_dialogIndex = 0;

            m_backgroundObj.SetActive(true);
            Update_Dialog();
        }

        private void Close_Dialog()
        {
            m_backgroundObj.SetActive(false);
        }

        IEnumerator Type_Text(int dialogIndex, TMP_Text currentText, GameObject arrow)
        {
            m_isTyping = true;
            m_cancelTyping = false;

            currentText.text = "";
            foreach (char letter in m_dialogs[dialogIndex].dialogText.ToCharArray())
            {
                if (m_cancelTyping)
                {
                    currentText.text = m_dialogs[dialogIndex].dialogText;
                    break;
                }

                currentText.text += letter;
                yield return new WaitForSeconds(m_typeSpeed);
            }

            m_isTyping = false;

            if (m_arrowCoroutine != null)
                StopCoroutine(m_arrowCoroutine);
            m_arrowCoroutine = StartCoroutine(Use_Arrow(arrow));

            yield break;
        }

        IEnumerator Use_Arrow(GameObject arrow)
        {
            while (false == m_isTyping)
            {
                arrow.SetActive(!arrow.activeSelf);
                yield return new WaitForSeconds(m_arrowSpeed);
            }
            yield break;
        }
        #endregion
    }
}

