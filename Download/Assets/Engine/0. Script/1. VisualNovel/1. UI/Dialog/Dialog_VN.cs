using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VisualNovel
{
    public class Dialog_VN : Dialog<DialogData_VN>
    {
        [Header("GameObject")]
        [SerializeField] private GameObject m_darkPanelObj;
        [SerializeField] private GameObject m_backgroundObj;
        [SerializeField] private GameObject[] m_standingObj;
        [SerializeField] private GameObject m_dialogBoxObj;
        [SerializeField] private TMP_Text m_nameTxt;
        [SerializeField] private TMP_Text m_dialogTxt;
        [SerializeField] private NpcLike m_heartScr;

        private Image m_backgroundImg;
        private Image[] m_standingImg;

        private int m_choiceIndex = 0;
        private List<GameObject> m_choice_Button = new List<GameObject>();

        private bool m_cutScene = false;
        public bool CutScene { set => m_cutScene = value; }

        private void Awake()
        {
            m_backgroundImg = m_backgroundObj.GetComponent<Image>();

            m_standingImg = new Image[m_standingObj.Length];
            for (int i = 0; i < m_standingObj.Length; i++)
                m_standingImg[i] = m_standingObj[i].GetComponent<Image>();
        }

        private void Update()
        {
            /*if(Input.GetKeyDown(KeyCode.F1))
            {
                m_dialogIndex = m_dialogs.Count - 1;
                m_isTyping    = false;
                Update_Dialog();
            }*/

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                Update_Dialog();

            // 버튼 존재 시 해당 입력 업데이트
            if (0 < m_choice_Button.Count && false == m_isTyping)
                Update_Button();
        }

        private void Update_Dialog(bool IsPonter = true)
        {
            // 커서가 UI 위치상에 존재할 시 반환
            if (IsPonter && EventSystem.current.IsPointerOverGameObject())
                return;

            if (m_isTyping)
                m_cancelTyping = true;
            else if (!m_isTyping)
            {
                // 다이얼로그 진행
                if (m_dialogIndex < m_dialogs.Count)
                {
                    switch (m_dialogs[m_dialogIndex].dialogEvent)
                    {
                        case DialogData_VN.DIALOGEVENT_TYPE.DET_NONE:
                            Update_None();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_FADEIN:
                            Update_FadeIn();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_FADEOUT:
                            Update_FadeOut();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_FADEOUTIN:
                            Update_FadeOutIn();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_STARTSHOOT:
                            Start_ShootGame();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_STARTCHASE:
                            Start_ChaseGame();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_PLAYCHASE:
                            Play_ChaseGame();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_LIKEADD:
                            Update_None();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_SHAKING:
                            Update_Shaking();
                            break;

                        case DialogData_VN.DIALOGEVENT_TYPE.DET_CUTSCENE:
                            Update_CutScene();
                            break;
                    }
                }
                else // 다이얼로그 종료
                {
                    if (0 < m_choice_Button.Count)
                        return;

                    Close_Dialog();
                }
            }
        }

        #region Update
        private void Update_Basic(int index)
        {
            m_dialogBoxObj.SetActive(true);

            // 다이얼로그 업데이트
            if (m_dialogs[index].owner == VisualNovelManager.NPCTYPE.OT_WHITE)
                m_nameTxt.text = GameManager.Ins.PlayerName;
            else
                m_nameTxt.text = m_dialogs[index].nameText;
            m_heartScr.Set_Owner(m_dialogs[index].owner); // 호감도 업데이트

            // 리소스 업데이트
            if (!string.IsNullOrEmpty(m_dialogs[index].backgroundSpr))
                m_backgroundImg.sprite = GameManager.Ins.Novel.BackgroundSpr[m_dialogs[index].backgroundSpr];
            Update_Standing(index);
        }

        private void Update_None(bool nextUpdate = false)
        {
            Update_Basic(m_dialogIndex);

            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_Text(m_dialogIndex, m_dialogTxt, /*m_arrowObj,*/ nextUpdate));
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
            if (!string.IsNullOrEmpty(m_dialogs[m_dialogIndex].choiceDialog[0]))
            {
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black,
                    () => Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>(m_dialogs[m_dialogIndex].choiceDialog[0])), 0f, false);
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

        private void Start_ShootGame()
        {
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black,
                () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_SHOOTGAME), 0.5f, false);
        }

        private void Start_ChaseGame()
        {
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black,
                () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_CHASEGAME), 0.5f, false);
        }

        private void Play_ChaseGame()
        {
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black,
                () => GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Chase>().Play_Level(), 0.5f, false);
        }

        private void Update_Shaking()
        {
            Update_None();

            // 카메라 쉐이킹
        }

        private void Update_CutScene()
        {
            if (m_cutScene == true)
                return;

            int eventCount = m_dialogs[m_dialogIndex].dialogCutScene.cutSceneEvents.Count;
            if (eventCount <= 0)
                return;

            m_cutScene = true;
            for (int i = 0; i < eventCount; ++i)
            {
                DialogCutScene.CUTSCENEEVENT_TYPE cutSceneEvent = m_dialogs[m_dialogIndex].dialogCutScene.cutSceneEvents[i];
                switch (cutSceneEvent)
                {
                    case DialogCutScene.CUTSCENEEVENT_TYPE.CET_BLINK:
                        Update_Blink((BasicValue)m_dialogs[m_dialogIndex].dialogCutScene.eventValues[i]);
                        break;

                    case DialogCutScene.CUTSCENEEVENT_TYPE.CET_CAMERA:
                        Update_Camera((CameraValue)m_dialogs[m_dialogIndex].dialogCutScene.eventValues[i]);
                        break;

                    case DialogCutScene.CUTSCENEEVENT_TYPE.CET_ANIMATION:
                        Update_Animation((AnimationValue)m_dialogs[m_dialogIndex].dialogCutScene.eventValues[i]);
                        break;

                    case DialogCutScene.CUTSCENEEVENT_TYPE.CET_LIKEPANEL:
                        StartCoroutine(Update_LikePanel());
                        break;

                    case DialogCutScene.CUTSCENEEVENT_TYPE.CET_ACTIVE:
                        Update_Active((ActiveValue)m_dialogs[m_dialogIndex].dialogCutScene.eventValues[i]);
                        break;
                }
            }
        }
        #endregion

        #region CutScene
        private void Update_Blink(BasicValue basicValue) // 인 아웃 // 인 아웃 // 인
        {
            m_dialogBoxObj.SetActive(false);
            GameManager.Ins.UI.Start_FadeIn(3.5f, Color.black, () => GameManager.Ins.UI.Start_FadeOut(2f, Color.black, () => GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Finish_CutScene(basicValue.nextIndex)), 0f, false)), 0f, false));
        }

        private void Update_Camera(CameraValue cameraValue)
        {
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);

            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            if (cameraValue.usePosition == true)
                camera.Start_Position(cameraValue.targetPosition, cameraValue.positionSpeed);
            if (cameraValue.useRotation == true)
                camera.Start_Rotation(cameraValue.targetRotation, cameraValue.rotationSpeed);

            StartCoroutine(Finish_Camera(camera, cameraValue.usePosition, cameraValue.useRotation, cameraValue.nextIndex));
        }

        private void Update_Animation(AnimationValue animationValue)
        {
            switch (animationValue.objectType)
            {
                case AnimationValue.OBJECT_TYPE.OJ_YANDERE:
                    GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Chase>().YandereAnimator.SetTrigger(animationValue.animatroTriger);
                    break;
            }

            m_cutScene = false;
            Update_None(animationValue.nextIndex);
        }

        private IEnumerator Update_LikePanel()
        {
            float time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (time >= 0.5f)
                {
                    GameManager.Ins.Novel.LikeabilityPanel.SetActive(true);
                    break;
                }

                yield return null;
            }

            time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (time >= 1.0f)
                {
                    GameManager.Ins.Novel.LikeabilityPanel.GetComponent<Likeability>().Shake_Heart();
                    break;
                }

                yield return null;
            }

            m_dialogIndex++;
            yield break;
        }

        private void Update_Active(ActiveValue activeValue)
        {
            switch(activeValue.objectType)
            {
                case ActiveValue.OBJECT_TYPE.OJ_SAW:
                    GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Chase>().Yandere.gameObject.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(activeValue.active);
                    break;
            }

            Finish_CutScene(activeValue.nextIndex);
        }

        private void Finish_CutScene(bool nextIndex)
        {
            m_cutScene = false;

            m_dialogIndex++;
            if (nextIndex == false)
                return;

            Update_Dialog();
        }

        private IEnumerator Finish_Camera(CameraCutscene camera, bool position, bool rotation, bool nextIndex)
        {
            while (true)
            {
                if (position == true && rotation == true)
                {
                    if (camera.IsPosition == false && camera.IsRotation == false)
                        break;
                }
                else if(position == true)
                {
                    if (camera.IsPosition == false)
                        break;
                }
                else if (rotation == true)
                {
                    if (camera.IsRotation == false)
                        break;
                }

                yield return null;
            }

            Finish_CutScene(nextIndex);
            yield break;
        }
        #endregion

        #region Each
        private void Update_Standing(int index)
        {
            switch (m_dialogs[index].standingSpr.Count)
            {
                case 0:
                    m_standingObj[0].SetActive(false);
                    m_standingObj[1].SetActive(false);
                    m_standingObj[2].SetActive(false);
                    break;

                case 1:
                    if (!string.IsNullOrEmpty(m_dialogs[index].standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[m_dialogs[index].standingSpr[0]];
                    }

                    m_standingObj[1].SetActive(false);
                    m_standingObj[2].SetActive(false);
                    break;

                case 2:
                    if (!string.IsNullOrEmpty(m_dialogs[index].standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[m_dialogs[index].standingSpr[0]];
                    }

                    if (!string.IsNullOrEmpty(m_dialogs[index].standingSpr[1]))
                    {
                        m_standingObj[1].SetActive(true);
                        m_standingObj[1].transform.localPosition = new Vector3(300.0f, -460.0f, 0.0f);
                        m_standingImg[1].sprite = GameManager.Ins.Novel.StandingSpr[m_dialogs[index].standingSpr[1]];
                    }
                    m_standingObj[2].SetActive(false);
                    break;

                case 3:
                    if (!string.IsNullOrEmpty(m_dialogs[index].standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[m_dialogs[index].standingSpr[0]];
                    }

                    if (!string.IsNullOrEmpty(m_dialogs[index].standingSpr[1]))
                    {
                        m_standingObj[1].SetActive(true);
                        m_standingObj[1].transform.localPosition = new Vector3(300.0f, -460.0f, 0.0f);
                        m_standingImg[1].sprite = GameManager.Ins.Novel.StandingSpr[m_dialogs[index].standingSpr[1]];
                    }

                    if (!string.IsNullOrEmpty(m_dialogs[index].standingSpr[2]))
                    { 
                        m_standingObj[2].SetActive(true);
                        m_standingObj[2].transform.localPosition = new Vector3(500.0f, -460.0f, 0.0f);
                        m_standingImg[2].sprite = GameManager.Ins.Novel.StandingSpr[m_dialogs[index].standingSpr[2]];
                    }
                    break;
            }
        }

        private void Create_ChoiceButton(int index)
        {
            m_darkPanelObj.SetActive(true);

            // 선택지 버튼 생성
            for (int i = 0; i < m_dialogs[index].choiceText.Count; ++i)
            {
                int ButtonIndex = i + 1; // 버튼 고유 인덱스

                GameObject Clone = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/UI/Button_Choice_VN");
                if (Clone)
                {
                    Clone.transform.SetParent(gameObject.transform);
                    Clone.transform.localPosition = new Vector3(0f, (130 + (i * -130)), 0f); // 130 / 0 / -130
                    Clone.transform.localScale = new Vector3(1f, 1f, 1f);

                    ButtonChoice_VN ButtonChoice = Clone.GetComponent<ButtonChoice_VN>();
                    ButtonChoice.ButtonIndex = i;
                    ButtonChoice.Ownerdialog = this;

                    TMP_Text TextCom = Clone.GetComponentInChildren<TMP_Text>();
                    if (TextCom)
                    {
                        TextCom.text = m_dialogs[index].choiceText[i];

                        Button button = Clone.GetComponent<Button>();
                        if (button) // 이벤트 핸들러 추가
                            button.onClick.AddListener(() => Click_Button(ButtonIndex));

                        m_choice_Button.Add(Clone);
                    }
                }
            }

            m_choiceIndex = 0;
            m_choice_Button[m_choiceIndex].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"];
        }

        private void Update_Button()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Click_Button(m_choiceIndex);
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_choiceIndex--;
                if (m_choiceIndex < 0)
                    m_choiceIndex = m_choice_Button.Count - 1;
                Set_Button();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_choiceIndex++;
                if (m_choiceIndex > m_choice_Button.Count - 1)
                    m_choiceIndex = 0;
                Set_Button();
            }
        }

        public void Enter_Button(int index)
        {
            m_choiceIndex = index;
            Set_Button();
        }

        private void Click_Button(int index)
        {
            switch (m_dialogs[m_dialogIndex - 1].choiceEventType[index - 1])
            {
                case DialogData_VN.CHOICEEVENT_TYPE.CET_CLOSE: // 다이얼로그 종료
                    Close_Dialog();
                    break;

                case DialogData_VN.CHOICEEVENT_TYPE.CET_DIALOG: // 다음 다이얼로그 불러오고 해당 다이얼로그로 이어서 출력
                    Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>(m_dialogs[m_dialogIndex - 1].choiceDialog[index - 1])); ;
                    break;
            }
        }

        public void Set_Button()
        {
            // 현재 인덱스 버튼을 제외한 모든 버튼 Off 이미지로 초기화
            for (int i = 0; i < m_choice_Button.Count; ++i)
            {
                if (i == m_choiceIndex)
                    m_choice_Button[i].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"]; // 버튼 On
                else
                    m_choice_Button[i].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonOFF"]; // 버튼 Off
            }
        }

        public void Close_Background()
        {
            m_backgroundObj.SetActive(false);
        }
        #endregion

        #region Common
        public void Start_Dialog(List<DialogData_VN> dialogs = null)
        {
            m_dialogs = dialogs;

            m_isTyping     = false;
            m_cancelTyping = false;
            m_dialogIndex = 0;
            m_choiceIndex = 0;

            m_darkPanelObj.SetActive(false);
            m_standingObj[0].SetActive(false);
            m_standingObj[1].SetActive(false);
            m_standingObj[2].SetActive(false);

            for (int i = 0; i < m_choice_Button.Count; ++i)
                Destroy(m_choice_Button[i]);
            m_choice_Button.Clear();

            m_backgroundObj.SetActive(true);
            gameObject.SetActive(true);

            Update_Dialog(false);
        }

        private void Close_Dialog()
        {
            gameObject.SetActive(false);
        }

        IEnumerator Type_Text(int index, TMP_Text currentText, bool nextUpdate)
        {
            // 플레이어 이름이 사용될 시 입력 받은 이름으로 변경
            m_dialogs[index].dialogText = m_dialogs[index].dialogText.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

            m_isTyping = true;
            m_cancelTyping = false;

            currentText.text = "";
            foreach (char letter in m_dialogs[index].dialogText.ToCharArray())
            {
                if (m_cancelTyping)
                {
                    currentText.text = m_dialogs[index].dialogText;
                    break;
                }

                currentText.text += letter;
                yield return new WaitForSeconds(m_typeSpeed);
            }

            m_isTyping = false;

            // 선택지 생성
            if (0 < m_dialogs[index].choiceText.Count)
                Create_ChoiceButton(index);

            // 호감도 증가
            if (m_dialogs[index].dialogEvent == DialogData_VN.DIALOGEVENT_TYPE.DET_LIKEADD)
            {
                GameManager.Ins.Novel.NpcHeart[(int)m_dialogs[index].owner]++;
                m_heartScr.Set_Owner(m_dialogs[index].owner);
            }

            // 업데이트
            if(nextUpdate == true)
                Update_Dialog();

            yield break;
        }
        #endregion
    }
}

